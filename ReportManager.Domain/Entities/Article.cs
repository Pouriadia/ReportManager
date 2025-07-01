using System;
using ReportManager.Domain.Interfaces;

namespace ReportManager.Domain.Entities
{
    public class Article : IEntity<Guid>
    {
        // Primary key
        public Guid Id { get; private set; }

        // Title of the article
        public string Title { get; private set; }

        // Full content of the article
        public string Content { get; private set; }

        // Short summary or description
        public string Summary { get; private set; }

        // When the article was published
        public DateTime PublishDate { get; private set; }

        // Importance level (e.g. 1,2,3)
        public int Importance { get; private set; }

        // Country associated with the article
        public string Country { get; private set; }

        // Foreign Key to Reporter
        public Guid ReporterId { get; private set; }

        // Parameterless constructor for EF Core
        private Article() { }

        /// <summary>
        /// Factory method to create a new Article.
        /// Ensures invariants: non-empty Title, Content, Summary, valid PublishDate.
        /// </summary>
        public static Article Create(
            string title,
            string content,
            string summary,
            DateTime publishDate,
            int importance,
            string country,
            Guid reporterId)
        {
            // Validate parameters (keep SRP & OCP)
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.", nameof(title));
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Content is required.", nameof(content));
            if (string.IsNullOrWhiteSpace(summary)) throw new ArgumentException("Summary is required.", nameof(summary));
            if (importance < 1 || importance > 5) throw new ArgumentOutOfRangeException(nameof(importance), "Importance must be between 1 and 5.");
            if (string.IsNullOrWhiteSpace(country)) throw new ArgumentException("Country is required.", nameof(country));

            return new Article
            {
                Id = Guid.NewGuid(),
                Title = title,
                Content = content,
                Summary = summary,
                PublishDate = publishDate,
                Importance = importance,
                Country = country,
                ReporterId = reporterId
            };
        }
    }
}