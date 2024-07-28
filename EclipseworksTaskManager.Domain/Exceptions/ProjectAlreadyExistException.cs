namespace EclipseworksTaskManager.Domain.Exceptions
{
    public class ProjectAlreadyExistException : Exception
    {
        public ProjectAlreadyExistException(string message) : base(message) { }
    }
}