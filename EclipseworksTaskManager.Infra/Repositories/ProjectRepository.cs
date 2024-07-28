using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Enums;
using EclipseworksTaskManager.Domain.Interfaces.Repository;
using EclipseworksTaskManager.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;

namespace EclipseworksTaskManager.Infra.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        public TaskManagerContext Context { get; set; }

        public ProjectRepository(TaskManagerContext context)
        {
            Context = context;
        }

        public async Task AddAsync(Project project)
        {
            await Context.AddAsync(project);
        }

        public async Task<bool> CheckForPendingJobs(Guid id)
        {
            return await Context.Jobs
                .AnyAsync(x =>
                    x.ProjectId == id &&
                    x.Status != JobStatusEnum.Done);
        }

        public void Delete(Guid id)
        {
            Context.Projects
                .Remove(new Project { Id = id });
        }

        public Task<List<Project>> GetAllByUserAsync(string userName)
        {
            return Context.Projects
                .Where(x => x.UserName == userName)
                .ToListAsync();
        }

        public async Task<Guid> CheckProjectExistence(string name)
        {
            var project = await Context.Projects
                .FirstOrDefaultAsync(x => x.Name == name);

            return project.Id;
        }

        public async Task<Project> GetByName(string name)
        {
            return await Context.Projects
                .FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}