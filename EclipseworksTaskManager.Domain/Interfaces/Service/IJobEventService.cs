using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Domain.Interfaces.Service
{
    public interface IJobEventService
    {
        Task AddAsync(JobEvent jobEvent);
    }
}