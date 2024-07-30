using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Exceptions;
using EclipseworksTaskManager.Domain.Interfaces;
using EclipseworksTaskManager.Domain.Interfaces.Service;

namespace EclipseworksTaskManager.Domain.Services
{
    public class JobCommentService : IJobCommentService
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public IUserService UserService { get; set; }

        public const string INVALID_DESCRIPTION_MESSAGE = "Description can not be null or empty.";

        public JobCommentService(IUnitOfWork unitOfWork, IUserService userService)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task AddAsync(JobEvent jobEvent)
        {
            if (string.IsNullOrWhiteSpace(jobEvent.Description))
                throw new ContractViolationException(INVALID_DESCRIPTION_MESSAGE);

            var job = await UnitOfWork.JobRepository
                .GetByIdAsync(jobEvent.JobId);

            if (job == null)
                throw new JobNotFoundException(JobService.JOB_NOT_FOUND_MESSAGE);

            jobEvent.UserName = UserService.Get();

            await UnitOfWork.JobEventRepository
                .AddAsync(jobEvent);

            await UnitOfWork.SaveChangesAsync();
        }
    }
}