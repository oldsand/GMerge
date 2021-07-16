using System;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Entities;
using GCommon.Archiving.Repositories;
using GCommon.Core.Utilities;
using GCommon.Data;
using GCommon.Data.Abstractions;
using GCommon.Data.Entities;
using GServer.Archestra.Abstractions;
using GServer.Services.Abstractions;
using NLog;

namespace GServer.Services
{
    public class ArchivingPipeline : PipelineProcessor<ChangeLog>
    {
        private readonly IGalaxyRepository _galaxyRepository;
        private readonly IGalaxyDataProviderFactory _dataProviderFactory;
        private readonly IArchiveRepositoryFactory _archiveRepositoryFactory;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _galaxyString;
        private readonly string _archiveString;

        public ArchivingPipeline(IGalaxyRepository galaxyRepository)
        {
            _galaxyRepository = galaxyRepository;
            
            _galaxyString = DbStringBuilder.GalaxyString(galaxyRepository.Name);
            _archiveString = DbStringBuilder.ArchiveString(galaxyRepository.Name);
        }

        public ArchivingPipeline(IGalaxyRepository galaxyRepository,
            IGalaxyDataProviderFactory dataProviderFactory,
            IArchiveRepositoryFactory archiveRepositoryFactory)
        {
            _galaxyRepository = galaxyRepository;
            _dataProviderFactory = dataProviderFactory;
            _archiveRepositoryFactory = archiveRepositoryFactory;
        }
        
        protected override ITargetBlock<ChangeLog> DefineFlow()
        {
            //1. Send change log to be stored in case the service stops without completion, we can pick back up where we left off (hopefully)
            //2. At the same time, use the log object id to retrieve the object from the database. We need this to construct archive object
            //3. After step 2 completes, use that result with the change log input to construct an archive object.
            //4. Use the resulting archive object to determine if it should be archived. (based on current archive settings)
            //5. Taking the results that pass step 4, Archive the current object from the galaxy repository.
            //6. Using the result from step 5, persist the archive object with log and entry to the database using Upsert.
            //7. If the archiving was successful, remove the associated log from the queue/storage.
            //8. If there was an issue, we probably want to pose failed entities somewhere where the use can review? Not sure yet.

            var broadcaster = new BroadcastBlock<ChangeLog>(log => log);
            var storeLog = new ActionBlock<ChangeLog>(StoreLog);
            var retrieveObject = new TransformBlock<ChangeLog, GObject>(RetrieveObject);
            var logBuffer = new BufferBlock<ChangeLog>();
            var objectLogPair = new JoinBlock<GObject, ChangeLog>();
            var generateObject = new TransformBlock<Tuple<GObject, ChangeLog>, ArchiveObject>(GenerateObject);
            var discardLog = new ActionBlock<Tuple<GObject, ChangeLog>>(DiscardLog);
            var isArchivable = new TransformBlock<ArchiveObject, ArchiveObject>(IsArchivable);
            var archive = new TransformBlock<ArchiveObject, ArchiveObject>(Archive);
            var removeLog = new ActionBlock<ArchiveObject>(RemoveLog);

            broadcaster.LinkTo(storeLog);
            broadcaster.LinkTo(retrieveObject);
            broadcaster.LinkTo(logBuffer);
            retrieveObject.LinkTo(objectLogPair.Target1);
            logBuffer.LinkTo(objectLogPair.Target2);
            objectLogPair.LinkTo(generateObject, tuple => tuple.Item1 != null && tuple.Item2 != null);
            objectLogPair.LinkTo(discardLog, tuple => tuple.Item2 != null);
            objectLogPair.LinkTo(DataflowBlock.NullTarget<Tuple<GObject, ChangeLog>>());
            generateObject.LinkTo(isArchivable);
            isArchivable.LinkTo(archive, o => o != null);
            isArchivable.LinkTo(DataflowBlock.NullTarget<ArchiveObject>(), o => o == null);
            
            //at this point done, but we want to remove queued log and any failures should be dealt with?
            
            return broadcaster;
        }
        
        private void StoreLog(ChangeLog log)
        {
            Logger.Trace("Generating archive log with id {ChangeLogId}", log.ChangeLogId);
            
            using var archive = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_archiveString)
                : _archiveRepositoryFactory.Create(_archiveString);
            
            archive.Queue.Enqueue(log.ChangeLogId);
        }

        private void DiscardLog(Tuple<GObject, ChangeLog> payload)
        {
            var log = payload.Item2;
            
            using var archive = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_archiveString)
                : _archiveRepositoryFactory.Create(_archiveString);

            archive.Queue.Dequeue(log.ChangeLogId);
        }
        
        private void RemoveLog(ArchiveObject obj)
        {
            using var archive = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_archiveString)
                : _archiveRepositoryFactory.Create(_archiveString);

            
            var log = obj.Logs.SingleOrDefault();
            if (log == null) return;

            archive.Queue.Dequeue(log.ChangeLogId);
        }
        
        private GObject RetrieveObject(ChangeLog changeLog)
        {
            Logger.Trace("Retrieving GObject with id {ObjectId}", changeLog.ObjectId);

            try
            {
                using var galaxy = _dataProviderFactory == null
                    ? new GalaxyDataProvider(_galaxyString)
                    : _dataProviderFactory.Create(_galaxyString);
            
                return galaxy.Objects.Find(changeLog.ObjectId);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to retrieve object with id {ObjectId}", changeLog.ObjectId);
                return null;
            }
        }

        private ArchiveObject GenerateObject(Tuple<GObject, ChangeLog> payload)
        {
            var (obj, log) = payload;
            Logger.Trace("Generating archive object with id {ObjectId}", obj.ObjectId);

            var archiveObject = new ArchiveObject(obj.ObjectId, obj.TagName, obj.ConfigVersion, obj.Template);
            archiveObject.AddLog(log.ChangeLogId, log.ChangeDate, log.Operation, log.Comment, log.UserName);
            return archiveObject;
        }
        
        private ArchiveObject IsArchivable(ArchiveObject archiveObject)
        {
            using var archive = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_archiveString)
                : _archiveRepositoryFactory.Create(_archiveString);

            return archive.IsArchivable(archiveObject) ? archiveObject : null;
        }

        private ArchiveObject Archive(ArchiveObject obj)
        {
            using var archive = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_archiveString)
                : _archiveRepositoryFactory.Create(_archiveString);

            try
            {
                using var archiver = new GalaxyArchiver(_galaxyRepository, archive);
                archiver.Archive(obj);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to archive object with id {ObjectId}", obj.ObjectId);
                throw;
            }
            
            return obj;
        }
    }
}