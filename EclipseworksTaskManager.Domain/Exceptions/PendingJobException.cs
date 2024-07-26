namespace EclipseworksTaskManager.Domain.Exceptions
{
    public class PendingJobException : Exception
    {
        public PendingJobException(string message) : base(message) { }
    }
}