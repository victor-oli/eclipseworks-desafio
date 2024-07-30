namespace EclipseworksTaskManager.Domain.Exceptions
{
    public class ContractViolationException : EtmBaseException
    {
        public ContractViolationException(string message) : base(message) { }
    }
}