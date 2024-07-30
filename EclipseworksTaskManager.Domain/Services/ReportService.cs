using EclipseworksTaskManager.Domain.Interfaces;
using EclipseworksTaskManager.Domain.Interfaces.Service;
using EclipseworksTaskManager.Domain.ValueObjects;
using Newtonsoft.Json;

namespace EclipseworksTaskManager.Domain.Services
{
    public class ReportService : IReportService
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public ReportService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<List<DoneJobsByUser>> GetDoneJobsByUserInTheLastThirtyDays()
        {
            var events = await UnitOfWork.JobEventRepository
                .GetAllInTheLastDays(30);

            var doneEvents = events
                .Where(x => CheckIfIsDoneJob(x.Description))
                .ToList();

            var result = new List<DoneJobsByUser>();

            foreach (var job in doneEvents)
            {
                var existingReportItem = result.FirstOrDefault(x => x.UserName == job.UserName);

                if (existingReportItem != null)
                {
                    existingReportItem.JobsDoneQuantity += 1;
                }
                else
                {
                    result.Add(new DoneJobsByUser
                    {
                        UserName = job.UserName,
                        JobsDoneQuantity = 1
                    });
                }
            }

            return result
                .OrderByDescending(x => x.JobsDoneQuantity)
                .ToList();
        }

        private bool CheckIfIsDoneJob(string description)
        {
            return JsonConvert
                .DeserializeObject<dynamic>(description)
                .To.Status == 2;
        }
    }
}