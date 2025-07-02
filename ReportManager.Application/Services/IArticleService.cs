using ReportManager.Application.DTOs;

namespace ReportManager.Application.Services
{
    /// <summary>
    /// Defines use cases related to articles.
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// Retrieves all articles sorted by publish date descending.
        /// </summary>
        /// <param name="limit">Maximum number of articles to return.</param>
        /// <param name="offset">Number of articles to skip (for paging).</param>
        /// <returns>Enumerable of ArticleDto.</returns>
        IEnumerable<ArticleDto> GetAllArticles(int limit = 100, int offset = 0);

        /// <summary>
        /// Retrieves articles for a given country on an exact date.
        /// </summary>
        /// <param name="country">Country name to filter by.</param>
        /// <param name="date">Exact date to filter by (date component only).</param>
        /// <returns>Enumerable of ArticleDto.</returns>
        IEnumerable<ArticleDto> GetArticlesByCountryAndDate(string country, DateTime date);

        /// <summary>
        /// Retrieves articles published in the last specified number of days.
        /// </summary>
        /// <param name="days">Number of days to look back.</param>
        /// <returns>Enumerable of ArticleDto.</returns>
        IEnumerable<ArticleDto> GetRecentArticles(int days);
    }
}