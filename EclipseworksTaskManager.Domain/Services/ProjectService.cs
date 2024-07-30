using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Exceptions;
using EclipseworksTaskManager.Domain.Interfaces;
using EclipseworksTaskManager.Domain.Interfaces.Service;

namespace EclipseworksTaskManager.Domain.Services
{
    public class ProjectService : IProjectService
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public const string PROJECT_ALREADY_EXIST_MESSAGE = "Already exist a project with this name. {0}";
        public const string TWENTY_JOBS_LIMIT_MESSAGE = "Currently a project cannot have more than twenty jobs. Please consider this.";
        public const string INVALID_PROJECT_NAME_MESSAGE = "Name can not be null or empty.";

        public ProjectService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task AddAsync(Project project)
        {
            if(string.IsNullOrWhiteSpace(project.Name))
                throw new ContractViolationException(INVALID_PROJECT_NAME_MESSAGE);

            if (project.Jobs.Count > 20)
                throw new JobsOffLimitException(TWENTY_JOBS_LIMIT_MESSAGE);

            var projectWithSameName = await UnitOfWork.ProjectRepository
                .GetByName(project.Name);

            if (projectWithSameName != null)
                throw new ProjectAlreadyExistException(string.Format(PROJECT_ALREADY_EXIST_MESSAGE, project.Name));

            await UnitOfWork.ProjectRepository.AddAsync(project);

            await UnitOfWork.SaveChangesAsync();
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

        public async Task<List<Project>> GetAllByUserAsync(string userName)
        {
            return await UnitOfWork.ProjectRepository
                .GetAllByUserAsync(userName);
        }
    }
}