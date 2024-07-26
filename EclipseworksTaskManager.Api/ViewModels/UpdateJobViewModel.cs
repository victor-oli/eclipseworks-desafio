using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Enums;

namespace EclipseworksTaskManager.Api.ViewModels
{
    public class UpdateJobViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public JobStatusEnum Status { get; set; }
        public bool IsEnabled { get; set; }

        public Job GetJob()
        {
            return new Job
            {
                Id = Id,
                Description = Description,
                IsEnabled = IsEnabled,
                Status = Status,
                Title = Title
            };
        }
    }
}