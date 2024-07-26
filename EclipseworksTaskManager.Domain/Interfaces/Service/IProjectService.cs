using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Domain.Interfaces.Service
{
    public interface IProjectService
    {
        Task AddAsync(Project project);
        Task Delete(Guid id);
        Task<List<Project>> GetAllByUserAsync(string userName);
    }
}