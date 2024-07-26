using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Api.ViewModels
{
    public class CreateProjectViewModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }

        public Project GetProject()
        {
            return new Project
            {
                Name = Name,
                UserName = UserName
            };
        }
    }
}