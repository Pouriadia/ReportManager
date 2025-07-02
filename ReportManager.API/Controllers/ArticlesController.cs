using Microsoft.AspNetCore.Mvc;
using ReportManager.Application.DTOs;
using ReportManager.Application.Services;

namespace ReportManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// GET api/articles?limit={limit}&offset={offset}
        /// Returns all articles sorted by publish date descending.
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<ArticleDto>> GetAll([FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var dtos = _articleService.GetAllArticles(limit, offset);
            return Ok(dtos);
        }

        /// <summary>
        /// GET api/articles/by-country?country={country}&date={date}
        /// Returns articles for a given country on the exact date.
        /// </summary>
        [HttpGet("by-country")]
        public ActionResult<IEnumerable<ArticleDto>> GetByCountryAndDate([FromQuery] string country, [FromQuery] DateTime date)
        {
            var dtos = _articleService.GetArticlesByCountryAndDate(country, date);
            return Ok(dtos);
        }

        /// <summary>
        /// GET api/articles/recent?days={days}
        /// Returns articles published in the last specified number of days.
        /// </summary>
        [HttpGet("recent")]
        public ActionResult<IEnumerable<ArticleDto>> GetRecent([FromQuery] int days = 1)
        {
            var dtos = _articleService.GetRecentArticles(days);
            return Ok(dtos);
        }
    }
}