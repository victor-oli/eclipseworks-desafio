namespace EclipseworksTaskManager.Domain.Exceptions
{
    public class JobsOffLimitException : Exception
    {
        public JobsOffLimitException(string message) : base(message) { }
    }
}