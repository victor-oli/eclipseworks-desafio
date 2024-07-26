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
        public async Task<List<GetAllByProjectIdViewModel>> GetAllByProjectId(string projectName)
        {
            var dbResult = await JobService.GetAllByProjectNameAsync(projectName);

            return dbResult
                .Select(x => new GetAllByProjectIdViewModel(x))
                .ToList();
        }

        [HttpPost]
        public async Task Create(CreateJobViewModel viewModel)
        {
            await JobService
                .AddAsync(viewModel.GetJob());
        }

        [HttpPut]
        public async Task Update(UpdateJobViewModel viewModel)
        {
            await JobService
                .Update(viewModel.GetJob());
        }

        [HttpPost("{jobId}/comment")]
        public async Task AddComment(Guid jobId, AddCommentViewModel viewModel)
        {
            await JobCommentService
                .AddAsync(viewModel.GetEvent(jobId));
        }

        [HttpDelete("{jobId}")]
        public async Task Delete(Guid jobId)
        {
            await JobService
                .Delete(jobId);
        }
    }
}