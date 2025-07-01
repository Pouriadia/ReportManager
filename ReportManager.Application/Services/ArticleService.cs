using System;
using System.Collections.Generic;
using System.Linq;
using ReportManager.Application.DTOs;
using ReportManager.Application.Interfaces;
using ReportManager.Domain.Entities;

namespace ReportManager.Application.Services
{
    /// <summary>
    /// Implementation of IArticleService, handling article-related use cases.
    /// </summary>
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IEnumerable<ArticleDto> GetAllArticles(int limit = 100, int offset = 0)
        {
            var entities = _articleRepository.GetAll(limit, offset);
            return entities.Select(MapToDto);
        }

        public IEnumerable<ArticleDto> GetArticlesByCountryAndDate(string country, DateTime date)
        {
            var entities = _articleRepository.GetByCountryAndDate(country, date);
            return entities.Select(MapToDto);
        }

        public IEnumerable<ArticleDto> GetRecentArticles(int days)
        {
            var entities = _articleRepository.GetRecent(days);
            return entities.Select(MapToDto);
        }

        private static ArticleDto MapToDto(Article a)
        {
            return new ArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Summary = a.Summary,
                PublishDate = a.PublishDate,
                Importance = a.Importance,
                Country = a.Country,
                ReporterId = a.ReporterId
            };
        }
    }
}