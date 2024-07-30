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
        public async Task<BaseRespose<object>> Create(CreateJobViewModel viewModel)
        {
            await JobService
                .AddAsync(viewModel.GetJob());

            return BaseRespose<object>
                .GetSuccess();
        }

        [HttpPut]
        public async Task<BaseRespose<object>> Update(UpdateJobViewModel viewModel)
        {
            await JobService
                .UpdateAsync(viewModel.GetJob());

            return BaseRespose<object>
                .GetSuccess();
        }

        [HttpPost("{jobId}/comment")]
        public async Task<BaseRespose<object>> AddComment(Guid jobId, AddCommentViewModel viewModel)
        {
            await JobCommentService
                .AddAsync(viewModel.GetEvent(jobId));

            return BaseRespose<object>
                .GetSuccess();
        }

        [HttpDelete("{jobId}")]
        public async Task<BaseRespose<object>> Delete(Guid jobId)
        {
            await JobService
                .DeleteAsync(jobId);

            return BaseRespose<object>
                .GetSuccess();
        }
    }
}