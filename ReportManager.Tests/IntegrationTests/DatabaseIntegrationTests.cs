using System.Linq;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ReportManager.Infrastructure.Data;
using ReportManager.Domain.Entities;
using Xunit;

namespace ReportManager.Tests.IntegrationTests
{
    public class DatabaseIntegrationTests
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
        public void DbContext_Creates_Articles_And_Reporters_Tables()
        {
            using var context = CreateContext();

            // Retrieve all table names defined in the EF Core model
            var tableNames = context.Model
                .GetEntityTypes()
                .Select(t => t.GetTableName())
                .ToList();

            // Verify that both "Articles" and "Reporters" tables exist
            tableNames.Should().Contain(new[] { "Articles", "Reporters" });
        }

        [Fact]
        public void DbContext_ArticlesTable_Has_Correct_Columns()
        {
            using var context = CreateContext();

            // Get EF Core metadata for the Article entity
            var articleEntity = context.Model.FindEntityType(typeof(Article));

            // Extract the column names for the Articles table
            var columnNames = articleEntity
                .GetProperties()
                .Select(p => p.GetColumnName())
                .ToList();

            // Verify that all expected columns are present
            columnNames.Should().Contain(new[]
            {
                "Id",
                "Title",
                "Content",
                "Summary",
                "PublishDate",
                "Importance",
                "Country",
                "ReporterId"
            });
        }

        [Fact]
        public void DbContext_ReportersTable_Has_Correct_Columns()
        {
            using var context = CreateContext();

            // Get EF Core metadata for the Reporter entity
            var reporterEntity = context.Model.FindEntityType(typeof(Reporter));

            // Extract the column names for the Reporters table
            var columnNames = reporterEntity
                .GetProperties()
                .Select(p => p.GetColumnName())
                .ToList();

            // Verify that all expected columns are present
            columnNames.Should().Contain(new[]
            {
                "Id",
                "FirstName",
                "LastName",
                "Email",
                "Phone",
                "HireDate",
                "Bio"
            });
        }
    }
}
