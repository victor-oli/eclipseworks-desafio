using EclipseworksTaskManager.Api.ViewModels;
using EclipseworksTaskManager.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace EclipseworksTaskManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        public IJobService JobService { get; set; }
        public IJobCommentService JobCommentService { get; set; }

        public JobController(
            IJobService jobService,
            IJobCommentService jobCommentService)
        {
            JobService = jobService;
            JobCommentService = jobCommentService;
        }

        [HttpGet("{projectName}/all")]
        public async Task<BaseRespose<List<GetAllByProjectIdViewModel>>> GetAllByProjectId(string projectName)
        {
            var dbResult = await JobService.GetAllByProjectNameAsync(projectName);

            var jobs = dbResult
                .Select(x => new GetAllByProjectIdViewModel(x))
                .ToList();

            return BaseRespose<List<GetAllByProjectIdViewModel>>.GetSuccess(jobs);
        }

        [HttpPost]
        public async Task Create(CreateJobViewModel viewModel)
        {
            await JobService
                .AddAsync(viewModel.GetJob());

            BaseRespose<object>
                .GetSuccess();
        }

        [HttpPut]
        public async Task Update(UpdateJobViewModel viewModel)
        {
            await JobService
                .UpdateAsync(viewModel.GetJob());

            BaseRespose<object>
                .GetSuccess();
        }

        [HttpPost("{jobId}/comment")]
        public async Task AddComment(Guid jobId, AddCommentViewModel viewModel)
        {
            await JobCommentService
                .AddAsync(viewModel.GetEvent(jobId));

            BaseRespose<object>
                .GetSuccess();
        }

        [HttpDelete("{jobId}")]
        public async Task Delete(Guid jobId)
        {
            await JobService
                .DeleteAsync(jobId);

            BaseRespose<object>
                .GetSuccess();
        }
    }
}