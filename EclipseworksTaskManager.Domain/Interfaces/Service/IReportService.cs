using EclipseworksTaskManager.Domain.ValueObjects;

namespace EclipseworksTaskManager.Domain.Interfaces.Service
{
    public interface IReportService
    {
        Task<List<DoneJobsByUser>> GetDoneJobsByUserInTheLastThirtyDays();
    }
}