namespace EclipseworksTaskManager.Domain.Exceptions
{
    public class ProjectNotFoundException : EtmBaseException
    {
        public ProjectNotFoundException(string message) : base(message) { }
    }
}