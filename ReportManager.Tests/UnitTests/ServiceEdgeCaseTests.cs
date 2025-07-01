using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using ReportManager.Application.Interfaces;
using ReportManager.Application.Services;
using ReportManager.Domain.Entities;
using Xunit;

namespace ReportManager.Tests.UnitTests
{
    public class ServiceEdgeCaseTests
    {
        [Fact]
        public void ArticleService_GetAllArticles_NoArticles_ReturnsEmpty()
        {
            var repo = new Mock<IArticleRepository>();
            repo.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<Article>());

            var service = new ArticleService(repo.Object);

            var result = service.GetAllArticles().ToList();
            result.Should().BeEmpty();
        }

        [Fact]
        public void ArticleService_GetByCountryAndDate_NoArticles_ReturnsEmpty()
        {
            var repo = new Mock<IArticleRepository>();
            repo.Setup(r => r.GetByCountryAndDate(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(new List<Article>());

            var service = new ArticleService(repo.Object);

            var result = service.GetArticlesByCountryAndDate("Any", DateTime.UtcNow).ToList();
            result.Should().BeEmpty();
        }

        [Fact]
        public void ArticleService_GetRecentArticles_NoArticles_ReturnsEmpty()
        {
            var repo = new Mock<IArticleRepository>();
            repo.Setup(r => r.GetRecent(It.IsAny<int>()))
                .Returns(new List<Article>());

            var service = new ArticleService(repo.Object);

            var result = service.GetRecentArticles(7).ToList();
            result.Should().BeEmpty();
        }

        [Fact]
        public void ReporterService_GetTopReporters_NoArticles_ReturnsEmpty()
        {
            var articleRepo = new Mock<IArticleRepository>();
            articleRepo.Setup(r => r.GetRecent(It.IsAny<int>()))
                       .Returns(new List<Article>());

            var reporterRepo = new Mock<IReporterRepository>();
            var service = new ReporterService(articleRepo.Object, reporterRepo.Object);

            var result = service.GetTopReporters(DateTime.UtcNow, 5).ToList();
            result.Should().BeEmpty();
        }
    }
}
