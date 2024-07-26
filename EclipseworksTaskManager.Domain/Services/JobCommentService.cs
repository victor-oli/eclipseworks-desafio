using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Interfaces;
using EclipseworksTaskManager.Domain.Interfaces.Service;

namespace EclipseworksTaskManager.Domain.Services
{
    public class JobCommentService : IJobCommentService
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public JobCommentService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task AddAsync(JobEvent jobEvent)
        {
            await UnitOfWork.JobEventRepository.AddAsync(jobEvent);

            await UnitOfWork.SaveChangesAsync();
        }
    }
}