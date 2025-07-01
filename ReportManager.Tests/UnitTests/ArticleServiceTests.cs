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
    public class ArticleServiceTests
    {
        private readonly Mock<IArticleRepository> _articleRepoMock;
        private readonly ArticleService _service;

        public ArticleServiceTests()
        {
            _articleRepoMock = new Mock<IArticleRepository>();
            _service = new ArticleService(_articleRepoMock.Object);
        }

        [Fact]
        public void GetAllArticles_ShouldReturnMappedDtos_InDescendingOrder()
        {
            // Arrange: prepare sample articles
            var now = DateTime.UtcNow;
            var entities = new List<Article>
            {
                Article.Create("Title1", "Content1", "Summary1", now.AddDays(-1), 1, "CountryA", Guid.NewGuid()),
                Article.Create("Title2", "Content2", "Summary2", now,           2, "CountryB", Guid.NewGuid())
            };
            _articleRepoMock
                .Setup(r => r.GetAll(10, 0))
                .Returns(entities.OrderByDescending(a => a.PublishDate));

            // Act
            var result = _service.GetAllArticles(limit: 10, offset: 0).ToList();

            // Assert
            result.Should().HaveCount(2);
            result[0].Title.Should().Be("Title2");
            result[1].Title.Should().Be("Title1");
            result.Select(d => d.Summary).Should().Contain(new[] { "Summary1", "Summary2" });
        }

        [Fact]
        public void GetArticlesByCountryAndDate_ShouldFilterAndMapCorrectly()
        {
            // Arrange
            var specificDate = new DateTime(2025, 6, 30);
            var entities = new List<Article>
            {
                Article.Create("T1", "C1", "S1", specificDate, 1, "Iran", Guid.NewGuid()),
                Article.Create("T2", "C2", "S2", specificDate, 2, "USA",  Guid.NewGuid()),
                Article.Create("T3", "C3", "S3", specificDate.AddDays(-1), 3, "Iran", Guid.NewGuid())
            };
            _articleRepoMock
                .Setup(r => r.GetByCountryAndDate("Iran", specificDate))
                .Returns(entities.Where(a => a.Country == "Iran" && a.PublishDate.Date == specificDate.Date));

            // Act
            var dtos = _service.GetArticlesByCountryAndDate("Iran", specificDate).ToList();

            // Assert
            dtos.Should().HaveCount(1);
            dtos[0].Title.Should().Be("T1");
        }

        [Fact]
        public void GetRecentArticles_ShouldReturnArticlesWithinDays()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var entities = new List<Article>
            {
                Article.Create("Old", "OldC", "OldS", now.AddDays(-5), 1, "Any", Guid.NewGuid()),
                Article.Create("Recent", "RecC", "RecS", now.AddDays(-1), 2, "Any", Guid.NewGuid())
            };
            _articleRepoMock
                .Setup(r => r.GetRecent(2))
                .Returns(entities.Where(a => a.PublishDate >= now.AddDays(-2)));

            // Act
            var dtos = _service.GetRecentArticles(2).ToList();

            // Assert
            dtos.Should().ContainSingle()
                .Which.Title.Should().Be("Recent");
        }
    }
}