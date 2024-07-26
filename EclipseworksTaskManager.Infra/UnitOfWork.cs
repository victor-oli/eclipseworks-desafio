using EclipseworksTaskManager.Domain.Interfaces;
using EclipseworksTaskManager.Domain.Interfaces.Repository;
using EclipseworksTaskManager.Infra.EntityConfig;

namespace EclipseworksTaskManager.Infra
{
    public class UnitOfWork : IUnitOfWork
    {
        public TaskManagerContext Context { get; set; }
        public IJobRepository JobRepository { get; set; }

        public IProjectRepository ProjectRepository { get; set; }

        public IJobEventRepository JobEventRepository { get; set; }

        public UnitOfWork(
            TaskManagerContext taskManagerContext, 
            IJobRepository jobRepository, 
            IProjectRepository projectRepository, 
            IJobEventRepository jobEventRepository)
        {
            JobRepository = jobRepository;
            ProjectRepository = projectRepository;
            JobEventRepository = jobEventRepository;
            Context = taskManagerContext;
        }

        public Task SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}