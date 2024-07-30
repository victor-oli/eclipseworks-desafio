using EclipseworksTaskManager.Domain.Entities;

namespace EclipseworksTaskManager.Domain.ValueObjects
{
    public class BeforeAfterJobUpdate
    {
        public Job Before { get; set; }
        public Job After { get; set; }
    }
}