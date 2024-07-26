namespace EclipseworksTaskManager.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public List<Job> Jobs { get; set; } = new();

        public bool CanHaveMoreJobs()
            => Jobs?.Count < 20;
    }
}