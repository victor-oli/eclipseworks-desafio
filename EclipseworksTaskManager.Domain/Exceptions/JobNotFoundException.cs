namespace EclipseworksTaskManager.Domain.Exceptions
{
    public class JobNotFoundException : EtmBaseException
    {
        public JobNotFoundException(string message) : base(message) { }
    }
}