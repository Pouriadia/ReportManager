using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ReportManager.Domain.Entities;
using ReportManager.Infrastructure.Data;

namespace ReportManager.Infrastructure.Services
{
    /// <summary>
    /// CSV importer that reads records and saves Articles and Reporters.
    /// </summary>
    public class CsvImporter : ICsvImporter
    {
        private readonly NewsDbContext _context;
        private readonly IPersianDateConverter _dateConverter;

        public CsvImporter(NewsDbContext context, IPersianDateConverter dateConverter)
        {
            _context = context;
            _dateConverter = dateConverter;
        }

        public void ImportArticles(Stream csvStream)
        {
            if (csvStream == null) throw new ArgumentNullException(nameof(csvStream));

            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            });

            var records = csv.GetRecords<dynamic>().ToList();
            foreach (var record in records)
            {
                string subject = record.Subject;
                string category = record.Category;
                string description = record.Description;
                string persianDate = record.PublishDate;
                string reporterFullName = record.ReporterFullName;
                string country = record.Country;
                var importance = int.TryParse(record.Importance?.ToString(), out int imp) ? imp : 1;

                DateTime publishDate = _dateConverter.ConvertToDateTime(persianDate);

                var names = reporterFullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                string firstName = names.Length > 0 ? names[0] : string.Empty;
                string lastName = names.Length > 1 ? names[1] : string.Empty;

                var reporter = _context.Reporters
                    .FirstOrDefault(r => r.FirstName == firstName && r.LastName == lastName);
                var dummyEmail = $"{firstName.ToLowerInvariant()}.{lastName.ToLowerInvariant()}@example.com";
                if (reporter == null)
                {
                    reporter = Reporter.Create(firstName, lastName, dummyEmail, string.Empty, DateTime.UtcNow, string.Empty);
                    _context.Reporters.Add(reporter);
                }

                var article = Article.Create(
                    title: subject,
                    content: description,
                    summary: description.Length > 200 ? description.Substring(0, 200) : description,
                    publishDate: publishDate,
                    importance: importance,
                    country: country,
                    reporterId: reporter.Id);

                _context.Articles.Add(article);
            }

            _context.SaveChanges();
        }
    }
}