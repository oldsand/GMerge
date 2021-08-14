using System;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Repositories;
using GCommon.Core.Utilities;
using GCommon.Data;
using GCommon.Data.Abstractions;
using GCommon.Data.Entities;
using GCommon.Primitives;
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
            var linkOptions = new DataflowLinkOptions {PropagateCompletion = true};

            var logBroadcast = new BroadcastBlock<ChangeLog>(log => log);
            var storeLog = new ActionBlock<ChangeLog>(StoreLog);
            var retrieveObject = new TransformBlock<ChangeLog, GalaxyObject>(RetrieveObject);
            var logBuffer = new BufferBlock<ChangeLog>();
            var objectLogPair = new JoinBlock<GalaxyObject, ChangeLog>(new GroupingDataflowBlockOptions {Greedy = false});
            var generateObject = new TransformBlock<Tuple<GalaxyObject, ChangeLog>, ArchiveObject>(GenerateObject);
            var objectBroadcast = new BroadcastBlock<ArchiveObject>(obj => obj);
            var objectBuffer = new BufferBlock<ArchiveObject>();
            var isArchivable = new TransformBlock<ArchiveObject, bool>(IsArchivable);
            var objectResultPair = new JoinBlock<ArchiveObject, bool>(new GroupingDataflowBlockOptions {Greedy = false});
            var validObject = new TransformBlock<Tuple<ArchiveObject, bool>, ArchiveObject>(tuple => tuple.Item1);
            var invalidObject = new TransformBlock<Tuple<ArchiveObject, bool>, ArchiveObject>(tuple => tuple.Item1);
            var archive = new TransformBlock<ArchiveObject, ArchiveObject>(Archive);
            var retrieveObjectNull = new ActionBlock<Tuple<GalaxyObject, ChangeLog>>(RemoveLog);
            var removeLog = new ActionBlock<ArchiveObject>(RemoveLog);

            logBroadcast.LinkTo(storeLog, linkOptions);
            logBroadcast.LinkTo(retrieveObject, linkOptions);
            logBroadcast.LinkTo(logBuffer, linkOptions);
            retrieveObject.LinkTo(objectLogPair.Target1, linkOptions);
            logBuffer.LinkTo(objectLogPair.Target2, linkOptions);
            objectLogPair.LinkTo(generateObject, linkOptions, tuple => tuple.Item1 != null && tuple.Item2 != null);
            objectLogPair.LinkTo(retrieveObjectNull, linkOptions, tuple => tuple.Item2 != null);
            generateObject.LinkTo(objectBroadcast, linkOptions);
            objectBroadcast.LinkTo(objectBuffer, linkOptions);
            objectBroadcast.LinkTo(isArchivable, linkOptions);
            objectBuffer.LinkTo(objectResultPair.Target1, linkOptions);
            isArchivable.LinkTo(objectResultPair.Target2, linkOptions);
            objectResultPair.LinkTo(validObject, linkOptions, tuple => tuple.Item2);
            objectResultPair.LinkTo(invalidObject, linkOptions, tuple => !tuple.Item2);
            validObject.LinkTo(archive, linkOptions);
            archive.LinkTo(removeLog, linkOptions);
            invalidObject.LinkTo(removeLog, linkOptions);
            
            AssignCompletion(removeLog);

            return logBroadcast;
        }

        private void StoreLog(ChangeLog log)
        {
            Logger.Trace("Generating archive log with id {ChangeLogId}", log.ChangeLogId);

            using var archive = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_archiveString)
                : _archiveRepositoryFactory.Create(_archiveString);

            var queued = 
                new QueuedLog(log.ChangeLogId, log.ObjectId, log.ChangeDate, log.Operation, log.Comment, log.UserName);
            archive.Queue.Enqueue(queued);
        }

        private void RemoveLog(Tuple<GalaxyObject, ChangeLog> payload)
        {
            var log = payload.Item2;

            using var archive = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_archiveString)
                : _archiveRepositoryFactory.Create(_archiveString);

            archive.Queue.Dequeue(log.ChangeLogId);
        }

        private void RemoveLog(ArchiveObject obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj), "obj can not be null");
            
            using var archive = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_archiveString)
                : _archiveRepositoryFactory.Create(_archiveString);

            var log = obj.Logs.SingleOrDefault();
            if (log == null) return;

            archive.Queue.Dequeue(log.ChangeLogId);
        }

        private GalaxyObject RetrieveObject(ChangeLog changeLog)
        {
            Logger.Trace("Retrieving GObject with id {ObjectId}", changeLog.ObjectId);

            using var galaxy = _dataProviderFactory == null
                ? new GalaxyDataProvider(_galaxyString)
                : _dataProviderFactory.Create(_galaxyString);

            return galaxy.Objects.Find(changeLog.ObjectId);
        }

        private ArchiveObject GenerateObject(Tuple<GalaxyObject, ChangeLog> payload)
        {
            var (obj, log) = payload;
            Logger.Trace("Generating archive object with id {ObjectId}", obj.ObjectId);

            var archiveObject = new ArchiveObject(obj.ObjectId, obj.TagName, obj.ConfigVersion, obj.Template);
            return archiveObject;
        }

        private bool IsArchivable(ArchiveObject archiveObject)
        {
            using var archive = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_archiveString)
                : _archiveRepositoryFactory.Create(_archiveString);

            return archive.IsArchivable(archiveObject);
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