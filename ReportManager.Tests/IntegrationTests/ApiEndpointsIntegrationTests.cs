using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using ReportManager.Application.DTOs; // for ArticleDto
using Xunit;

namespace ReportManager.Tests.IntegrationTests
{
    public class ApiEndpointsIntegrationTests : IClassFixture<ApiTestFixture>
    {
        private readonly HttpClient _client;

        public ApiEndpointsIntegrationTests(ApiTestFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GET_api_articles_returns_all_articles_ordered()
        {
            // Act
            var list = await _client.GetFromJsonAsync<List<ArticleDto>>("/api/articles");

            // Assert
            list.Should().NotBeNull();
            list.Count.Should().Be(2);
            list[0].Title.Should().Be("Fresh News");
        }

        [Fact]
        public async Task GET_api_articles_recent_returns_only_last_day()
        {
            // Act
            var list = await _client.GetFromJsonAsync<List<ArticleDto>>("/api/articles/recent?days=1");

            // Assert
            list.Should().NotBeNull();
            list.Should().ContainSingle(a => a.Title == "Fresh News");
        }

        [Fact]
        public async Task GET_api_articles_by_country_and_date_filters_correctly()
        {
            // Arrange
            var today = DateTime.UtcNow.ToString("yyyy-MM-dd");

            // Act
            var list = await _client.GetFromJsonAsync<List<ArticleDto>>(
                $"/api/articles/by-country?country=Iran&date={today}");

            // Assert
            list.Should().ContainSingle(a => a.Title == "Fresh News");
        }

        [Fact]
        public async Task GET_api_reporters_top_returns_correct_number_of_items()
        {
            // Arrange
            var since = DateTime.UtcNow.AddYears(-1).ToString("yyyy-MM-dd");

            // Act
            var response = await _client.GetAsync($"/api/reporters/top?since={since}&limit=5");

            // Assert status code
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Parse response JSON
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            // The root should be an array
            var items = doc.RootElement.EnumerateArray().ToList();

            // Expect exactly 2 items (as seeded in ApiTestFixture)
            items.Should().HaveCount(2);

            // Each element must be an object (even if empty)
            items.All(i => i.ValueKind == JsonValueKind.Object).Should().BeTrue();
        }
    }
}
