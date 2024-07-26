namespace EclipseworksTaskManager.Domain.Entities
{
    public class JobEvent
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserName { get; set; }

        public Guid JobId { get; set; }
        public Job Job { get; set; }
    }
}