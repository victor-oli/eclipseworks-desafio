using EclipseworksTaskManager.Domain.Entities;
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

        public JobService(IUnitOfWork unitOfWork, IUserService userService)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public Task AddAsync(Job job)
        {
            var count = UnitOfWork.JobRepository
                .GetCountByProjectId(job.ProjectId)
                .Result;

            if (count > 19)
                throw new JobsOffLimitException("Currently a project cannot have more than twenty jobs. Please consider this.");

            job.DueDate = DateTime.Now;
            job.IsEnabled = true;

            UnitOfWork.JobRepository
                .AddAsync(job);

            return UnitOfWork
                .SaveChangesAsync();
        }

        public void Delete(Job job)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Job job)
        {
            var originalJob = UnitOfWork.JobRepository
                .GetByIdAsync(job.Id).Result;

            var originalJobToLog = JsonConvert.SerializeObject(originalJob);

            originalJob.Description = job.Description;
            originalJob.Status = job.Status;
            originalJob.IsEnabled = job.IsEnabled;
            originalJob.Title = job.Title;

            await UnitOfWork.JobRepository.Update(originalJob);

            await UnitOfWork.JobEventRepository.AddAsync(new JobEvent
            {
                CreationDate = DateTime.Now,
                Description = $"The Job {originalJob.Title} has changed from {originalJobToLog} to {JsonConvert.SerializeObject(originalJob)}.",
                UserName = UserService.Get(),
                JobId = originalJob.Id
            });

            await UnitOfWork.SaveChangesAsync();
        }

        public Task<IList<Job>> GetAllByProjectNameAsync(string projectName)
        {
            return UnitOfWork.JobRepository
                .GetAllByProjectNameAsync(projectName);
        }
    }
}