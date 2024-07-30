using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Enums;
using EclipseworksTaskManager.Domain.Exceptions;

namespace EclipseworksTaskManager.Api.ViewModels
{
    public class UpdateJobViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public bool IsEnabled { get; set; }

        public Job GetJob()
        {
            if (!Enum.TryParse(typeof(JobStatusEnum), Status, out var parsedStatus) || !Enum.IsDefined(typeof(JobStatusEnum), parsedStatus))
                throw new ContractViolationException(CreateJobViewModel.STATUS_INVALID_MESSAGE);

            return new Job
            {
                Id = Id,
                Description = Description,
                IsEnabled = IsEnabled,
                Status = (JobStatusEnum)parsedStatus,
                Title = Title
            };
        }
    }
}