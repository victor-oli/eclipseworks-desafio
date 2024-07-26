using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Api.ViewModels
{
    public class AddCommentViewModel
    {
        public string Comment { get; set; }

        public JobEvent GetEvent(Guid jobId)
        {
            return new JobEvent
            {
                JobId = jobId,
                CreationDate = DateTime.Now,
                Description = Comment
            };
        }
    }
}
