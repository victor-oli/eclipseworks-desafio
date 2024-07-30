using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Interfaces.Repository;
using EclipseworksTaskManager.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;

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

        public Task<List<JobEvent>> GetAllInTheLastDays(int days)
        {
            var referenceDate = DateTime.Now;

            return Context.JobEvents
                .Where(x =>
                    x.CreationDate >= referenceDate.Date.AddDays(-days) && 
                    x.CreationDate <= referenceDate)
                .ToListAsync();
        }
    }
}