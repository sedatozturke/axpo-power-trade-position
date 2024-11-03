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
        public async Task<IActionResult> GetReports()
        {
            _reportService.GetReports();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReport(string id)
        {
            _reportService.GetReport(id);
            return Ok();
        }
    }
}
