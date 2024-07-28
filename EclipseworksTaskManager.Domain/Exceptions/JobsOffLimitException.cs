namespace EclipseworksTaskManager.Domain.Exceptions
{
    public class JobsOffLimitException : EtmBaseException
    {
        public JobsOffLimitException(string message) : base(message) { }
    }
}