using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Domain.Interfaces.Service
{
    public interface IJobService
    {
        Task AddAsync(Job job);
        Task Update(Job job);
        Task Delete(Guid jobId);
        Task<IList<Job>> GetAllByProjectNameAsync(string projectName);
    }
}