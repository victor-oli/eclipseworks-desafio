using AutoFixture.Idioms;
using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Services;
using EclipseworksTaskManager.UnitTests.Domain.AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace EclipseworksTaskManager.UnitTests.Domain.Services
{
    public class ReportServiceTests
    {
        [Theory, CustomAutoData]
        public void Sut_ShouldGuardItsClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ReportService).GetConstructors());
        }

        [Theory, CustomAutoData]
        public async Task GetDoneJobsByUserInTheLastThirtyDays_ShouldReturnCorrectly(
            ReportService sut)
        {
            var events = new List<JobEvent>
            {
                new JobEvent
                {
                    UserName = "victor",
                    Description = JsonConvert.SerializeObject(new
                    {
                        After = new
                        {
                            Status = 1
                        }
                    })
                },
                new JobEvent
                {
                    UserName = "victor",
                    Description = JsonConvert.SerializeObject(new
                    {
                        After = new
                        {
                            Status = 2
                        }
                    })
                },
                new JobEvent
                {
                    UserName = "victor",
                    Description = JsonConvert.SerializeObject(new
                    {
                        After = new
                        {
                            Status = 2
                        }
                    })
                },
                new JobEvent
                {
                    UserName = "Zé",
                    Description = JsonConvert.SerializeObject(new
                    {
                        After = new
                        {
                            Status = 2
                        }
                    })
                },
                new JobEvent
                {
                    UserName = "Zé",
                    Description = "comentário para os testes"
                }
            };

            sut.UnitOfWork.JobEventRepository
                .GetAllInTheLastDays(30)
                .Returns(events);

            var result = await sut.GetDoneJobsByUserInTheLastThirtyDays();

            result
                .Should()
                .HaveCount(2);

            result[0].UserName
                .Should()
                .Be("victor");

            result[0].JobsDoneQuantity
                .Should()
                .Be(2);

            result[1].UserName
                .Should()
                .Be("Zé");

            result[1].JobsDoneQuantity
                .Should()
                .Be(1);

            await sut.UnitOfWork.JobEventRepository
                .Received(1)
                .GetAllInTheLastDays(30);
        }
    }
}