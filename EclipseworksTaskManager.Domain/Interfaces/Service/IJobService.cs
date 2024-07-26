using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Domain.Interfaces.Service
{
    public interface IJobService
    {
        Task AddAsync(Job job);
        Task UpdateAsync(Job job);
        Task DeleteAsync(Guid jobId);
        Task<IList<Job>> GetAllByProjectNameAsync(string projectName);
    }
}