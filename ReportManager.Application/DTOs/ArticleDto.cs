using System;

namespace ReportManager.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Article entity.
    /// Used to carry article data between layers and to the API.
    /// </summary>
    public class ArticleDto
    {
        /// <summary>
        /// Unique identifier of the article.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the article.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Short summary or description of the article.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Date and time when the article was published.
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Importance level of the article (e.g. 1–5).
        /// </summary>
        public int Importance { get; set; }

        /// <summary>
        /// Country associated with the article.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Identifier of the reporter who authored the article.
        /// </summary>
        public Guid ReporterId { get; set; }
    }
}