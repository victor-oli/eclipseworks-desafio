using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Interfaces.Repository;
using EclipseworksTaskManager.Infra.EntityConfig;

namespace EclipseworksTaskManager.Infra.Repositories
{
    public class JobEventRepository : IJobEventRepository
    {
        public TaskManagerContext Context { get; set; }

        public JobEventRepository(TaskManagerContext context)
        {
            Context = context;
        }

        public async Task AddAsync(JobEvent jobEvent)
        {
            jobEvent.Id = Guid.NewGuid();

            await Context.JobEvents.AddAsync(jobEvent);
        }
    }
}