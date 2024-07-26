using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Domain.Interfaces.Service
{
    public interface IJobService
    {
        Task AddAsync(Job job);
        Task Update(Job job);
        void Delete(Job job);
        Task<IList<Job>> GetAllByProjectNameAsync(string projectName);
    }
}