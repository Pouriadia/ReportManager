using System;
using System.Collections.Generic;
using ReportManager.Domain.Entities;

namespace ReportManager.Application.Interfaces
{
    /// <summary>
    /// Repository contract for managing Article entities.
    /// </summary>
    public interface IArticleRepository
    {
        /// <summary>
        /// Retrieve a single article by its unique identifier.
        /// </summary>
        /// <param name="id">The Guid of the article.</param>
        /// <returns>The matching Article, or null if not found.</returns>
        Article GetById(Guid id);

        /// <summary>
        /// Retrieve all articles, ordered by publish date descending.
        /// </summary>
        /// <param name="limit">Maximum number of articles to return.</param>
        /// <param name="offset">Number of articles to skip (for paging).</param>
        /// <returns>Enumerable of Articles.</returns>
        IEnumerable<Article> GetAll(int limit = 100, int offset = 0);

        /// <summary>
        /// Retrieve all articles for a given country on an exact date.
        /// </summary>
        /// <param name="country">Country name to filter by.</param>
        /// <param name="date">Exact publish date to filter by (date component only).</param>
        /// <returns>Enumerable of Articles.</returns>
        IEnumerable<Article> GetByCountryAndDate(string country, DateTime date);

        /// <summary>
        /// Retrieve all articles published in the last N days.
        /// </summary>
        /// <param name="days">Number of days to look back.</param>
        /// <returns>Enumerable of Articles.</returns>
        IEnumerable<Article> GetRecent(int days);
    }
}