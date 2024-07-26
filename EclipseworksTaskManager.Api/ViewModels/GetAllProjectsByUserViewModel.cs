using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Api.ViewModels
{
    public class GetAllProjectsByUserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public GetAllProjectsByUserViewModel(Project project)
        {
            Id = project.Id;
            Name = project.Name;
        }
    }
}