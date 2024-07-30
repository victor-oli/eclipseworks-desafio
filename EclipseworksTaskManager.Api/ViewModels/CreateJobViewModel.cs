using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Enums;
using EclipseworksTaskManager.Domain.Exceptions;

namespace EclipseworksTaskManager.Api.ViewModels
{
    public class CreateJobViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public Guid ProjectId { get; set; }

        public const string STATUS_INVALID_MESSAGE = "This status is invalid.";
        public const string PRIORITY_INVALID_MESSAGE = "This priority is invalid.";

        public Job GetJob()
        {
            if (!Enum.TryParse(typeof(JobStatusEnum), Status, out var parsedStatus) || !Enum.IsDefined(typeof(JobStatusEnum), parsedStatus))
                throw new ContractViolationException(STATUS_INVALID_MESSAGE);

            if (!Enum.TryParse(typeof(PriorityEnum), Priority, out var parsedPriority) || !Enum.IsDefined(typeof(PriorityEnum), parsedPriority))
                throw new ContractViolationException(PRIORITY_INVALID_MESSAGE);

            return new Job
            {
                Description = Description,
                Title = Title,
                ProjectId = ProjectId,
                Status = (JobStatusEnum)parsedStatus,
                Priority = (PriorityEnum)parsedPriority
            };
        }
    }
}