using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReportManager.Application.Interfaces;
using ReportManager.Domain.Entities;
using ReportManager.Infrastructure.Data;

namespace ReportManager.Infrastructure.Services
{
    public class SourceDataImporter : ISourceDataImporter
    {
        private readonly NewsDbContext _destination;
        private readonly ICsvImporter    _csvImporter;

        public SourceDataImporter(NewsDbContext destination, ICsvImporter csvImporter)
        {
            _destination = destination;
            _csvImporter = csvImporter;
        }

        public async Task ImportAllAsync(string folderPath)
        {
            // 1. Ensure schema
            await _destination.Database.EnsureCreatedAsync();

            // 2. CSV files
            foreach (var path in Directory.GetFiles(folderPath, "*.csv"))
            {
                using var fs = File.OpenRead(path);
                _csvImporter.ImportArticles(fs);
            }

            // 3. SQLite source DB files
            foreach (var path in Directory.GetFiles(folderPath, "*.db"))
            {
                var options = new DbContextOptionsBuilder<NewsDbContext>()
                    .UseSqlite($"Data Source={path}")
                    .Options;

                using var src = new NewsDbContext(options);
                var articles  = await src.Articles.AsNoTracking().ToListAsync();
                var reporters = await src.Reporters.AsNoTracking().ToListAsync();

                // upsert reporters
                foreach (var r in reporters)
                {
                    if (! _destination.Reporters.Any(x => x.Id == r.Id))
                        _destination.Reporters.Add(r);
                }

                // upsert articles
                foreach (var a in articles)
                {
                    if (! _destination.Articles.Any(x => x.Id == a.Id))
                        _destination.Articles.Add(a);
                }
            }

            await _destination.SaveChangesAsync();
        }
    }
}
