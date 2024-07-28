using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Domain.Interfaces.Repository
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project);
        void Delete(Guid id);
        Task<bool> CheckForPendingJobs(Guid id);
        Task<List<Project>> GetAllByUserAsync(string userName);
        Task<Guid> CheckProjectExistence(string name);
        Task<Project> GetByName(string name);
    }
}