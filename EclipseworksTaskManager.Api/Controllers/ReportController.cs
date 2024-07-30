using EclipseworksTaskManager.Api.ViewModels;
using EclipseworksTaskManager.Domain.Interfaces.Service;
using EclipseworksTaskManager.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace EclipseworksTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        public IReportService ReportService { get; set; }

        public ReportController(IReportService reportService)
        {
            ReportService = reportService;
        }

        [HttpGet("DoneJobs")]
        public async Task<BaseRespose<List<DoneJobsByUser>>> GetDoneJobsByUserInTheLastThirtyDays()
        {
            var report = await ReportService
                .GetDoneJobsByUserInTheLastThirtyDays();

            return BaseRespose<List<DoneJobsByUser>>
                .GetSuccess(report);
        }
    }
}