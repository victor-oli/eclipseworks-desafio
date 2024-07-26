using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Domain.Interfaces.Service
{
    public interface IJobCommentService
    {
        Task AddAsync(JobEvent jobEvent);
    }
}