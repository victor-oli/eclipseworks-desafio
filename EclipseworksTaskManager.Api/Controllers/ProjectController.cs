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
        public async Task<List<GetAllProjectsByUserViewModel>> GetAllByUser(string userName)
        {
            try
            {
                var projects = await ProjectService
                    .GetAllByUserAsync(userName);

                return projects
                    .Select(x => new GetAllProjectsByUserViewModel(x))
                    .ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task Create(CreateProjectViewModel viewModel)
        {
            await ProjectService
                .AddAsync(viewModel.GetProject());
        }

        [HttpDelete("{projectId}")]
        public async Task Delete(Guid projectId)
        {
            await ProjectService
                .Delete(projectId);
        }
    }
}