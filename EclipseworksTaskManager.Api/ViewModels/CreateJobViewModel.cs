using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Enums;

namespace EclipseworksTaskManager.Api.ViewModels
{
    public class CreateJobViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public JobStatusEnum Status { get; set; }
        public PriorityEnum Priority { get; set; }
        public Guid projectId { get; set; }

        public Job GetJob()
        {
            return new Job
            {
                Description = Description,
                Priority = Priority,
                Status = Status,
                Title = Title,
                ProjectId = projectId
            };
        }
    }
}