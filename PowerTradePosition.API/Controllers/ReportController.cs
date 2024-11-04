using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerTradePosition.API.Services;

namespace PowerTradePosition.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetReports([FromQuery] string? search)
        {
            var reports = _reportService.GetReports(search);
            return Ok(reports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReport(string id)
        {
            var report = _reportService.GetReport(id);

            if (report is not null) {
                return Ok(report);
            }
            return NotFound();
        }
    }
}
