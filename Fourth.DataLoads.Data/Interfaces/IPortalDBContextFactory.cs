﻿using System.Threading.Tasks;

namespace Fourth.DataLoads.Data

{
    /// <summary>
    /// Manages the creation of database context classes for the People System.
    /// </summary>
    /// <remarks>This interface allows us to inject mock database contexts into the repositories.</remarks>
    public interface IPortalDBContextFactory
    {
        /// <summary>
        /// Creates a new database context that can be used for a single transaction.
        /// </summary>
        /// <param name="groupID">The groupID (AKA organisationID) to return a context for - i.e. the People System customer.</param>
        /// <returns>A database context.</returns>
        Task<PortalDBContext> GetContextAsync(int groupID);
    }
}