using ReportManager.Application.DTOs;
using ReportManager.Application.Interfaces;

namespace ReportManager.Application.Services
{
    /// <summary>
    /// Implementation of IReporterService, handling reporter-related use cases.
    /// </summary>
    public class ReporterService : IReporterService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IReporterRepository _reporterRepository;

        public ReporterService(
            IArticleRepository articleRepository,
            IReporterRepository reporterRepository)
        {
            _articleRepository = articleRepository;
            _reporterRepository = reporterRepository;
        }

        public IEnumerable<(ReporterDto Reporter, int ArticleCount)> GetTopReporters(DateTime since, int limit)
        {
            // Calculate days back and fetch recent articles
            int days = (int)(DateTime.UtcNow - since).TotalDays;
            var recentArticles = _articleRepository.GetRecent(days);

            // Group by reporter and count
            var counts = recentArticles
                .GroupBy(a => a.ReporterId)
                .Select(g => new { ReporterId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(limit)
                .ToList();

            // Map to DTOs
            var result = new List<(ReporterDto, int)>();
            foreach (var item in counts)
            {
                var entity = _reporterRepository.GetById(item.ReporterId);
                if (entity == null) continue;

                var dto = new ReporterDto
                {
                    Id = entity.Id,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                    Phone = entity.Phone,
                    HireDate = entity.HireDate
                };

                result.Add((dto, item.Count));
            }

            return result;
        }
    }
}