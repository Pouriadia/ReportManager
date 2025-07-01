using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using ReportManager.Application.DTOs;
using ReportManager.Application.Services;
using ReportManager.Application.Interfaces;
using ReportManager.Domain.Entities;
using Xunit;

namespace ReportManager.Tests.UnitTests
{
    public class ReporterServiceTests
    {
        private readonly Mock<IArticleRepository> _articleRepoMock;
        private readonly Mock<IReporterRepository> _reporterRepoMock;
        private readonly ReporterService _service;

        public ReporterServiceTests()
        {
            _articleRepoMock = new Mock<IArticleRepository>();
            _reporterRepoMock = new Mock<IReporterRepository>();
            _service = new ReporterService(_articleRepoMock.Object, _reporterRepoMock.Object);
        }

        [Fact]
        public void GetTopReporters_ShouldReturnCorrectlyGroupedAndSortedReporters()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var since = now.AddDays(-30);
            // Create article entities with reporter IDs
            var reporter1 = Reporter.Create("Alice", "Smith", "a@x.com", "123", now.AddYears(-1), "bio");
            var reporter2 = Reporter.Create("Bob", "Jones", "b@y.com", "456", now.AddYears(-1), "bio");
            
            // reporter1 has 3 articles, reporter2 has 1
            var articles = new List<Article>
            {
                Article.Create("T1", "C1", "S1", now.AddDays(-1), 1, "Any", reporter1.Id),
                Article.Create("T2", "C2", "S2", now.AddDays(-2), 1, "Any", reporter1.Id),
                Article.Create("T3", "C3", "S3", now.AddDays(-3), 1, "Any", reporter1.Id),
                Article.Create("T4", "C4", "S4", now.AddDays(-4), 1, "Any", reporter2.Id)
            };

            _articleRepoMock
                .Setup(r => r.GetRecent(It.IsAny<int>()))
                .Returns(articles);

            // Setup reporter repo to return the reporter entities
            _reporterRepoMock.Setup(r => r.GetById(reporter1.Id)).Returns(reporter1);
            _reporterRepoMock.Setup(r => r.GetById(reporter2.Id)).Returns(reporter2);

            // Act
            var result = _service.GetTopReporters(since, limit: 2).ToList();

            // Assert
            result.Should().HaveCount(2);
            result[0].Reporter.Id.Should().Be(reporter1.Id);
            result[0].ArticleCount.Should().Be(3);
            result[1].Reporter.Id.Should().Be(reporter2.Id);
            result[1].ArticleCount.Should().Be(1);
        }
    }
}