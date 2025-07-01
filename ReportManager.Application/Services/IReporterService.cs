using System;
using System.Collections.Generic;
using ReportManager.Application.DTOs;

namespace ReportManager.Application.Services
{
    /// <summary>
    /// Defines use cases related to reporters.
    /// </summary>
    public interface IReporterService
    {
        /// <summary>
        /// Retrieves top reporters by number of articles since the given date.
        /// </summary>
        /// <param name="since">Date to look back from.</param>
        /// <param name="limit">Maximum number of top reporters to return.</param>
        /// <returns>
        /// Enumerable of (ReporterDto, article count) tuples.
        /// </returns>
        IEnumerable<(ReporterDto Reporter, int ArticleCount)> GetTopReporters(DateTime since, int limit);
    }
}