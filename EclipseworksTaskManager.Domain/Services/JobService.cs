using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Enums;
using EclipseworksTaskManager.Domain.Exceptions;
using EclipseworksTaskManager.Domain.Interfaces;
using EclipseworksTaskManager.Domain.Interfaces.Service;
using Newtonsoft.Json;

namespace EclipseworksTaskManager.Domain.Services
{
    public class JobService : IJobService
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public IUserService UserService { get; set; }

        public const string JOB_OF_LIMIT_EXCEPTION_MESSAGE = "Currently a project cannot have more than twenty jobs. Please consider this.";
        public const string PROJECT_NOT_FOUND_MESSAGE = "Project not found.";
        public const string NULL_TITLE_MESSAGE = "Name property can not be null or empty.";
        public const string JOB_NOT_FOUND_MESSAGE = "Job not found.";

        public JobService(IUnitOfWork unitOfWork, IUserService userService)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task AddAsync(Job job)
        {
            if (string.IsNullOrWhiteSpace(job.Title))
                throw new ContractViolationException(NULL_TITLE_MESSAGE);

            var count = await UnitOfWork.JobRepository
                .GetCountByProjectId(job.ProjectId);

            if (count > 19)
                throw new JobsOffLimitException(JOB_OF_LIMIT_EXCEPTION_MESSAGE);

            var project = await UnitOfWork.ProjectRepository
                .GetByIdUntracked(job.ProjectId);

            if (project == null)
                throw new ProjectNotFoundException(PROJECT_NOT_FOUND_MESSAGE);

            job.DueDate = DateTime.Now;
            job.IsEnabled = true;

            await UnitOfWork.JobRepository
                .AddAsync(job);

            await UnitOfWork
                .SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid jobId)
        {
            UnitOfWork.JobRepository
                .Delete(new Job { Id = jobId });

            await UnitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Job job)
        {
            var originalJob = await UnitOfWork.JobRepository
                .GetByIdAsync(job.Id);

            if (originalJob == null)
                throw new JobNotFoundException(JOB_NOT_FOUND_MESSAGE);

            var originalJobToLog = JsonConvert.SerializeObject(originalJob);

            originalJob.Description = job.Description;
            originalJob.Status = job.Status;
            originalJob.IsEnabled = job.IsEnabled;
            originalJob.Title = job.Title;

            await UnitOfWork.JobEventRepository.AddAsync(new JobEvent
            {
                CreationDate = DateTime.Now,
                Description = $"The Job {originalJob.Title} has changed from {originalJobToLog} to {JsonConvert.SerializeObject(originalJob)}.",
                JobId = originalJob.Id
            });

            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<IList<Job>> GetAllByProjectNameAsync(string projectName)
        {
            return await UnitOfWork.JobRepository
                .GetAllByProjectNameAsync(projectName);
        }
    }
}