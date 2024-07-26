using EclipseworksTaskManager.Domain.Enums;

namespace EclipseworksTaskManager.Domain.Entities
{
    public class Job
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public JobStatusEnum Status { get; set; }
        public PriorityEnum Priority { get; set; }
        public bool IsEnabled { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public List<JobEvent> JobEvents { get; set; } = new();
    }
}