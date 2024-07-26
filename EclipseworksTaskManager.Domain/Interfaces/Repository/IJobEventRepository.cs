using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Domain.Interfaces.Repository
{
    public interface IJobEventRepository
    {
        Task AddAsync(JobEvent jobEvent); 
    }
}