using EclipseworksTaskManager.Domain.Interfaces.Repository;

namespace EclipseworksTaskManager.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IJobRepository JobRepository { get; }
        IProjectRepository ProjectRepository { get; }
        IJobEventRepository JobEventRepository { get; }

        Task SaveChangesAsync();
    }
}