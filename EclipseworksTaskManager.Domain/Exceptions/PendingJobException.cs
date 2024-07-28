namespace EclipseworksTaskManager.Domain.Exceptions
{
    public class PendingJobException : EtmBaseException
    {
        public PendingJobException(string message) : base(message) { }
    }
}