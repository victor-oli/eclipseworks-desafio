using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Exceptions;
using EclipseworksTaskManager.Domain.Interfaces;
using EclipseworksTaskManager.Domain.Interfaces.Service;

namespace EclipseworksTaskManager.Domain.Services
{
    public class ProjectService : IProjectService
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public ProjectService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task AddAsync(Project project)
        {
            if (project.Jobs.Count > 20)
                throw new JobsOffLimitException("Currently a project cannot have more than twenty jobs. Please consider this.");

            UnitOfWork.ProjectRepository.AddAsync(project);

            return UnitOfWork.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var hasPendingJobs = await UnitOfWork.ProjectRepository
                .CheckForPendingJobs(id);

            if (hasPendingJobs)
                throw new PendingJobException("This Project cannot be deleted while there is any job pending. Please remove or finish the jobs before trying again.");

            UnitOfWork.ProjectRepository.Delete(id);

            await UnitOfWork.SaveChangesAsync();
        }

        public Task<List<Project>> GetAllByUserAsync(string userName)
        {
            return UnitOfWork.ProjectRepository
                .GetAllByUserAsync(userName);
        }
    }
}