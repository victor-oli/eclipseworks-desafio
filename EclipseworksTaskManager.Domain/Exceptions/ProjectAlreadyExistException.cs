namespace EclipseworksTaskManager.Domain.Exceptions
{
    public class ProjectAlreadyExistException : EtmBaseException
    {
        public ProjectAlreadyExistException(string message) : base(message) { }
    }
}