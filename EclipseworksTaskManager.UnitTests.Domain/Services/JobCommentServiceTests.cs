using AutoFixture.Idioms;
using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Services;
using EclipseworksTaskManager.UnitTests.Domain.AutoFixture;
using NSubstitute;
using Xunit;

namespace EclipseworksTaskManager.UnitTests.Domain.Services
{
    public class JobCommentServiceTests
    {
        [Theory, CustomAutoData]
        public void Sut_ShouldGuardItsClauses(GuardClauseAssertion assertion)
        {
            assertion
                .Verify(typeof(JobCommentService).GetConstructors());
        }

        [Theory, CustomAutoData]
        public async Task AddAsync_ShouldAddCorrectly(
            JobCommentService sut,
            JobEvent jobEvent)
        {
            sut.UnitOfWork.JobEventRepository
                .AddAsync(jobEvent)
                .Returns(Task.CompletedTask);

            sut.UnitOfWork
                .SaveChangesAsync()
                .Returns(Task.CompletedTask);

            await sut.AddAsync(jobEvent);

            await sut.UnitOfWork.JobEventRepository
                .Received(1)
                .AddAsync(jobEvent);

            await sut.UnitOfWork
                .Received(1)
                .SaveChangesAsync();
        }
    }
}