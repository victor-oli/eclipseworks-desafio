using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Api.ViewModels
{
    public class GetAllByProjectIdViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string DueDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public bool IsEnabled { get; set; }
        public string ProjectName { get; set; }

        public GetAllByProjectIdViewModel(Job job)
        {
            Title = job.Title;
            Description = job.Description;
            DueDate = job.DueDate.ToString("dd/MM/yyyy HH:mm:ss");
            IsEnabled = job.IsEnabled;
            Priority = job.Priority.ToString();
            ProjectName = job.Project.Name;
            Status = job.Status.ToString();
        }
    }
}