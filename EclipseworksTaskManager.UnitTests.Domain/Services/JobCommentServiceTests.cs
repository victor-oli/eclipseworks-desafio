using AutoFixture.Idioms;
using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Exceptions;
using EclipseworksTaskManager.Domain.Services;
using EclipseworksTaskManager.UnitTests.Domain.AutoFixture;
using FluentAssertions;
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
            string userName,
            Job job,
            JobCommentService sut,
            JobEvent jobEvent)
        {
            job.Id = jobEvent.JobId;

            sut.UserService
                .Get()
                .Returns(userName);

            sut.UnitOfWork.JobRepository
                .GetByIdAsync(jobEvent.JobId)
                .Returns(job);

            sut.UnitOfWork.JobEventRepository
                .AddAsync(jobEvent)
                .Returns(Task.CompletedTask);

            sut.UnitOfWork
                .SaveChangesAsync()
                .Returns(Task.CompletedTask);

            await sut.AddAsync(jobEvent);

            jobEvent.UserName
                .Should()
                .Be(userName);

            job.Id
                .Should()
                .Be(jobEvent.JobId);

            sut.UserService
                .Received(1)
                .Get();

            await sut.UnitOfWork.JobRepository
                .Received(1)
                .GetByIdAsync(jobEvent.JobId);

            await sut.UnitOfWork.JobEventRepository
                .Received(1)
                .AddAsync(jobEvent);

            await sut.UnitOfWork
                .Received(1)
                .SaveChangesAsync();
        }

        [Theory]
        [CustomInlineAutoData(null)]
        [CustomInlineAutoData("")]
        [CustomInlineAutoData("   ")]
        public async Task AddAsync_WithInvalidDescription_ShouldThrowException(
            string description,
            JobCommentService sut,
            JobEvent jobEvent)
        {
            jobEvent.Description = description;

            Func<Task> addAsync = async () => await sut.AddAsync(jobEvent);

            await addAsync
                .Should()
                .ThrowExactlyAsync<ContractViolationException>()
                .WithMessage(JobCommentService.INVALID_DESCRIPTION_MESSAGE);

            await sut.UnitOfWork.JobRepository
                .DidNotReceive()
                .GetByIdAsync(jobEvent.JobId);

            sut.UserService
                .DidNotReceive()
                .Get();

            await sut.UnitOfWork.JobEventRepository
                .DidNotReceive()
                .AddAsync(jobEvent);

            await sut.UnitOfWork
                .DidNotReceive()
                .SaveChangesAsync();
        }
    }
}