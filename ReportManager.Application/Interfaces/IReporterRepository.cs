using System;
using System.Collections.Generic;
using ReportManager.Domain.Entities;

namespace ReportManager.Application.Interfaces
{
    /// <summary>
    /// Repository contract for managing Reporter entities.
    /// </summary>
    public interface IReporterRepository
    {
        /// <summary>
        /// Retrieve a single reporter by its unique identifier.
        /// </summary>
        /// <param name="id">The Guid of the reporter.</param>
        /// <returns>The matching Reporter, or null if not found.</returns>
        Reporter GetById(Guid id);

        /// <summary>
        /// Retrieve all reporters.
        /// </summary>
        /// <returns>Enumerable of Reporters.</returns>
        IEnumerable<Reporter> GetAll();
    }
}