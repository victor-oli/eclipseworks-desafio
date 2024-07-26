using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Domain.Interfaces.Repository
{
    public interface IJobRepository
    {
        Task<Job> GetByIdAsync(Guid id);
        Task<IList<Job>> GetAllByProjectNameAsync(string projectName);
        Task AddAsync(Job job);
        void Delete(Job job);
        Task<int> GetCountByProjectId(Guid projectId);
    }
}