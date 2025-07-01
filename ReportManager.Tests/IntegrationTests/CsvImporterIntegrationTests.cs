using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ReportManager.Infrastructure.Data;
using ReportManager.Infrastructure.Services; // for CsvImporter & PersianDateConverter
using ReportManager.Domain.Entities;
using Xunit;

namespace ReportManager.Tests.IntegrationTests
{
    public class CsvImporterIntegrationTests
    {
        private NewsDbContext CreateContext()
        {
            // Create an in-memory SQLite connection
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            // Configure DbContext to use the in-memory SQLite
            var options = new DbContextOptionsBuilder<NewsDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new NewsDbContext(options);

            // Ensure the database schema is created
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public void CsvImporter_Imports_Articles_And_Reporters_From_Csv()
        {
            // Arrange: prepare DbContext, converter, and importer
            using var context = CreateContext();
            var converter = new PersianDataConverter();
            var importer  = new CsvImporter(context, converter);

            // Build CSV content with header and sample rows
            var sb = new StringBuilder();
            sb.AppendLine("Row,Subject,Category,Description,PublishDate,ReporterFullName,Country,Importance");
            sb.AppendLine("1,Test Title,TestCat,Some description,1404/03/24,John Doe,Iran,3");
            sb.AppendLine("2,Another Title,TestCat2,Other description,1404/03/25,Jane Smith,USA,2");

            // Convert the CSV string to a MemoryStream
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));

            // Act: perform the CSV import
            importer.ImportArticles(stream);

            // Assert: verify that two reporters were created
            var reporters = context.Reporters.ToList();
            reporters.Should().HaveCount(2);
            reporters.Should().Contain(r => r.FirstName == "John" && r.LastName == "Doe");
            reporters.Should().Contain(r => r.FirstName == "Jane" && r.LastName == "Smith");

            // Assert: verify that two articles were created
            var articles = context.Articles.ToList();
            articles.Should().HaveCount(2);
            articles.Select(a => a.Title)
                    .Should().Contain(new[] { "Test Title", "Another Title" });
        }
    }
}
