using Microsoft.AspNetCore.Mvc;
using ReportManager.Application.Services;
using ReportManager.API.DTOs;

namespace ReportManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportersController : ControllerBase
    {
        private readonly IReporterService _reporterService;

        public ReportersController(IReporterService reporterService)
            => _reporterService = reporterService;

        [HttpGet("top")]
        public ActionResult<IEnumerable<TopReporterDto>> GetTop(
            [FromQuery] DateTime since,
            [FromQuery] int limit = 5)
        {
            // دریافت تاپل‌های (ReporterDto, count)
            var data = _reporterService.GetTopReporters(since, limit);

            // نگاشت به DTO
            var result = data.Select(x => new TopReporterDto
                {
                    Id           = x.Reporter.Id,
                    FirstName    = x.Reporter.FirstName,
                    LastName     = x.Reporter.LastName,
                    ArticleCount = x.ArticleCount
                })
                .ToList();

            return Ok(result);
        }
    }
}