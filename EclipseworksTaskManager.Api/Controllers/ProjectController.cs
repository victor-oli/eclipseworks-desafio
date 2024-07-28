using EclipseworksTaskManager.Api.ViewModels;
using EclipseworksTaskManager.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace EclipseworksTaskManager.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        public IProjectService ProjectService { get; set; }

        public ProjectController(IProjectService projectService)
        {
            ProjectService = projectService;
        }

        [HttpGet("{userName}")]
        public async Task<BaseRespose<List<GetAllProjectsByUserViewModel>>> GetAllByUser(string userName)
        {
            var projects = await ProjectService
                .GetAllByUserAsync(userName);

            var result = projects
                .Select(x => new GetAllProjectsByUserViewModel(x))
                .ToList();

            return BaseRespose<List<GetAllProjectsByUserViewModel>>
                .GetSuccess(result);
        }

        [HttpPost]
        public async Task Create(CreateProjectViewModel viewModel)
        {
            await ProjectService
                .AddAsync(viewModel.GetProject());

            BaseRespose<object>
                .GetSuccess();
        }

        [HttpDelete("{projectId}")]
        public async Task Delete(Guid projectId)
        {
            await ProjectService
                .Delete(projectId);

            BaseRespose<object>
                .GetSuccess();
        }
    }
}