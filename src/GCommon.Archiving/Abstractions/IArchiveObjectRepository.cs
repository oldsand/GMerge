using System;
using System.Collections.Generic;
using GCommon.Primitives;

namespace GCommon.Archiving.Abstractions
{
    /// <summary>
    /// Defines the api for interacting with archive object entities in a given archive database
    /// </summary>
    public interface IArchiveObjectRepository : IDisposable
    {
        /// <summary>
        /// Determines if the given archive object exists in the archive database
        /// </summary>
        /// <param name="objectId">The objectId of the object to check</param>
        /// <returns></returns>
        bool Exists(int objectId);
        
        /// <summary>
        /// Gets the specified object from the database 
        /// </summary>
        /// <param name="objectId">The objectId of the object to retrieve</param>
        /// <returns></returns>
        ArchiveObject Get(int objectId);
        
        /// <summary>
        /// Gets all the archive objects that exist in the database
        /// </summary>
        /// <returns></returns>
        IEnumerable<ArchiveObject> GetAll();
        
        /// <summary>
        /// Finds the objects in the database with the given tag name
        /// </summary>
        /// <param name="tagName">The tag name of the objects to find</param>
        /// <returns></returns>
        IEnumerable<ArchiveObject> FindByTagName(string tagName);
        
        /// <summary>
        /// Inserts or updates the database with the provided object. This call add the object if it does not already
        /// exist, and update it if it does.
        /// </summary>
        /// <param name="archiveObject">The object to insert or update</param>
        void Upsert(ArchiveObject archiveObject);
        
        /// <summary>
        /// Deletes the specified object from the database
        /// </summary>
        /// <param name="objectId">The objectId of the object to delete</param>
        void Delete(int objectId);
    }
}