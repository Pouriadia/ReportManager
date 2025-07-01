using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReportManager.API;
using ReportManager.Infrastructure.Data;

namespace ReportManager.Tests.IntegrationTests
{
    public class ApiTestFixture : WebApplicationFactory<Program>
    {
        private SqliteConnection _connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            builder.ConfigureServices(services =>
            {
                var descriptor = services.Single(d =>
                    d.ServiceType == typeof(DbContextOptions<NewsDbContext>));
                services.Remove(descriptor);

                services.AddDbContext<NewsDbContext>(options =>
                {
                    options.UseSqlite(_connection);
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<NewsDbContext>();
                ctx.Database.EnsureCreated();

                // Seed sample data:
                var rep1 = Domain.Entities.Reporter.Create(
                    "Alice", "Wonder", "alice.wonder@example.com", "", DateTime.UtcNow.AddYears(-1), "");
                var rep2 = Domain.Entities.Reporter.Create(
                    "Bob", "Builder", "bob.builder@example.com", "", DateTime.UtcNow.AddMonths(-6), "");
                ctx.Reporters.AddRange(rep1, rep2);

                var art1 = Domain.Entities.Article.Create(
                    "Fresh News", "content", "content", DateTime.UtcNow, 5, "Iran", rep1.Id);
                var art2 = Domain.Entities.Article.Create(
                    "Old News", "content", "content", DateTime.UtcNow.AddDays(-2), 3, "USA", rep2.Id);
                ctx.Articles.AddRange(art1, art2);

                ctx.SaveChanges();
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _connection?.Close();
                _connection?.Dispose();
            }
        }
    }
}
