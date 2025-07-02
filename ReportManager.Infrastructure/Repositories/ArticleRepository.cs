using ReportManager.Application.Interfaces;
using ReportManager.Domain.Entities;
using ReportManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ReportManager.Infrastructure.Repositories
{
    /// <summary>
    /// EF Core implementation of IArticleRepository.
    /// </summary>
    public class ArticleRepository : IArticleRepository
    {
        private readonly NewsDbContext _context;

        public ArticleRepository(NewsDbContext context)
        {
            _context = context;
        }

        public Article GetById(Guid id)
        {
            return _context.Articles
                .AsNoTracking()
                .FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Article> GetAll(int limit = 100, int offset = 0)
        {
            return _context.Articles
                .AsNoTracking()
                .OrderByDescending(a => a.PublishDate)
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

        public IEnumerable<Article> GetByCountryAndDate(string country, DateTime date)
        {
            return _context.Articles
                .AsNoTracking()
                .Where(a => a.Country == country &&
                            a.PublishDate.Date == date.Date)
                .OrderByDescending(a => a.PublishDate)
                .ToList();
        }

        public IEnumerable<Article> GetRecent(int days)
        {
            var cutoff = DateTime.UtcNow.AddDays(-days);
            return _context.Articles
                .AsNoTracking()
                .Where(a => a.PublishDate >= cutoff)
                .OrderByDescending(a => a.PublishDate)
                .ToList();
        }
    }
}