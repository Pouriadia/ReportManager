using Microsoft.EntityFrameworkCore;
using ReportManager.Domain.Entities;
using ReportManager.Infrastructure.Data.Configurations;

namespace ReportManager.Infrastructure.Data
{
    /// <summary>
    /// EF Core DbContext for News Analysis system, configured for SQLite.
    /// </summary>
    public class NewsDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Reporter> Reporters { get; set; }

        public NewsDbContext(DbContextOptions<NewsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ArticleConfiguration());
            modelBuilder.ApplyConfiguration(new ReporterConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}