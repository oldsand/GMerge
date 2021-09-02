using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GCommon.Primitives;

namespace GCommon.Archiving.Abstractions
{
    public interface IArchiveRepository : IDisposable
    {
        /// <summary>
        /// Gets the current archive config object
        /// </summary>
        /// <returns></returns>
        ArchiveConfig GetConfig();
        
        /// <summary>
        /// Determines if the given archive object is able to be archived under the current archive settings
        /// </summary>
        /// <param name="archiveObject"></param>
        /// <returns></returns>
        bool IsArchivable(ArchiveObject archiveObject);
        
        /// <summary>
        /// Determines if the given archive object exists in the archive database
        /// </summary>
        /// <param name="objectId">The objectId of the object to check</param>
        /// <returns></returns>
        bool ObjectExists(int objectId);
        
        /// <summary>
        /// Gets the specified object from the database 
        /// </summary>
        /// <param name="objectId">The objectId of the object to retrieve</param>
        /// <returns></returns>
        ArchiveObject GetObject(int objectId);
        
        /// <summary>
        /// Gets all the archive objects that exist in the database
        /// </summary>
        /// <returns></returns>
        IEnumerable<ArchiveObject> GetObjects();
        
        /// <summary>
        /// Finds the objects in the database with the given tag name
        /// </summary>
        /// <param name="tagName">The tag name of the objects to find</param>
        /// <returns></returns>
        IEnumerable<ArchiveObject> GetObjects(string tagName);
        
        /// <summary>
        /// Inserts or updates the database with the provided object. This call add the object if it does not already
        /// exist, and update it if it does.
        /// </summary>
        /// <param name="archiveObject">The object to insert or update</param>
        void UpsertObject(ArchiveObject archiveObject);
        
        /// <summary>
        /// Deletes the specified object from the database
        /// </summary>
        /// <param name="objectId">The objectId of the object to delete</param>
        void DeleteObject(int objectId);
        
        /// <summary>
        /// Gets all entries
        /// </summary>
        /// <returns></returns>
        IEnumerable<ArchiveEntry> GetEntries();
        
        /// <summary>
        /// Finds entries that satisfy the provided predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<ArchiveEntry> FindEntries(Expression<Func<ArchiveEntry, bool>> predicate);
        
        /// <summary>
        /// Gets the log with the specified change log id
        /// </summary>
        /// <param name="changeLogId"></param>
        /// <returns></returns>
        ArchiveLog GetLog(int changeLogId);
        
        /// <summary>
        /// Gets all archive logs
        /// </summary>
        /// <returns></returns>
        IEnumerable<ArchiveLog> GetLogs();
        
        /// <summary>
        /// Finds logs that satisfy the provided predicate expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<ArchiveLog> FindLogs(Expression<Func<ArchiveLog, bool>> predicate);
        
        /// <summary>
        /// Gets the queued log with the given change log id
        /// </summary>
        /// <param name="changelogId"></param>
        /// <returns></returns>
        QueuedLog GetQueuedLog(int changelogId);
        
        /// <summary>
        /// Adds a queued log to the archive database
        /// </summary>
        /// <param name="log">The log to add</param>
        void Enqueue(QueuedLog log);
        
        /// <summary>
        /// Removes a queued log from the archive database
        /// </summary>
        /// <param name="changeLogId">The id of the log to remove</param>
        void Dequeue(int changeLogId);
        
        /// <summary>
        /// Indicates whether there are pending changes that need to be saved 
        /// </summary>
        /// <returns></returns>
        bool HasChanges();
        
        /// <summary>
        /// Saves any changes to the archive database
        /// </summary>
        /// <returns></returns>
        int Save();
        
        /// <summary>
        /// Asynchronously saves changes to the archive database
        /// </summary>
        /// <returns></returns>
        Task<int> SaveAsync();
    }
}