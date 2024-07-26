using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Interfaces.Repository;
using EclipseworksTaskManager.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;

namespace EclipseworksTaskManager.Infra.Repositories
{
    public class JobRepository : IJobRepository
    {
        public TaskManagerContext Context { get; set; }

        public JobRepository(TaskManagerContext taskManagerContext)
        {
            Context = taskManagerContext;
        }

        public async Task AddAsync(Job job)
        {
            job.Id = Guid.NewGuid();

            await Context.Jobs.AddAsync(job);
        }

        public async Task<IList<Job>> GetAllByProjectNameAsync(string projectName)
        {
            return await Context.Jobs
                .Where(x => x.Project.Name == projectName)
                .Include(x => x.Project)
                .ToListAsync();
        }

        public void Delete(Job job)
        {
            throw new NotImplementedException();
        }

        public async Task<Job> GetByIdAsync(Guid id)
        {
            return await Context.Jobs
                .FirstAsync(x => x.Id == id);
        }

        public async Task<int> GetCountByProjectId(Guid projectId)
        {
            return await Context.Jobs
                .CountAsync(x => x.Project.Id == projectId);
        }
    }
}