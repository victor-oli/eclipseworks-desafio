using AutoFixture.Idioms;
using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.Domain.Exceptions;
using EclipseworksTaskManager.Domain.Services;
using EclipseworksTaskManager.Infra;
using EclipseworksTaskManager.UnitTests.Domain.AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace EclipseworksTaskManager.UnitTests.Domain.Services
{
    public class JobServiceTests
    {
        [Theory, CustomAutoData]
        public void Sut_ShouldGuardItsClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(JobService).GetConstructors());
        }

        [Theory]
        [CustomInlineAutoData(20)]
        [CustomInlineAutoData(21)]
        public async Task AddAsync_WhenAProjectHasTwentyJobsOrMore_ShouldThrowException(
            short count,
            Job job,
            JobService sut)
        {
            // arrange
            sut.UnitOfWork.JobRepository
                .GetCountByProjectId(job.ProjectId)
                .Returns(count);

            // act
            Func<Task> addAsync = async () => await sut.AddAsync(job);

            // assert
            await addAsync
                .Should()
                .ThrowExactlyAsync<JobsOffLimitException>()
                .WithMessage(JobService.JOB_OF_LIMIT_EXCEPTION_MESSAGE);

            await sut.UnitOfWork.JobRepository
                .Received(1)
                .GetCountByProjectId(job.ProjectId);

            await sut.UnitOfWork.JobRepository
                .DidNotReceive()
                .AddAsync(Arg.Any<Job>());

            await sut.UnitOfWork
                .DidNotReceive()
                .SaveChangesAsync();
        }

        [Theory, CustomAutoData]
        public async Task AddAsync_WhenProjectHasLessThanTwentyJobs_ShouldAddCorrectly(
            Job job,
            Project project,
            JobService sut)
        {
            // arrange
            job.IsEnabled = false;
            job.DueDate = default;

            sut.UnitOfWork.JobRepository
                .GetCountByProjectId(job.ProjectId)
                .Returns((short)10);

            sut.UnitOfWork.ProjectRepository
                .GetByIdUntracked(job.ProjectId)
                .Returns(project);

            sut.UnitOfWork.JobRepository
                .AddAsync(job)
                .Returns(Task.CompletedTask);

            sut.UnitOfWork
                .SaveChangesAsync()
                .Returns(Task.CompletedTask);

            // act
            await sut.AddAsync(job);

            // assert
            job.IsEnabled
                .Should()
                .BeTrue();

            job.DueDate
                .Should()
                .NotBeSameDateAs(default);

            await sut.UnitOfWork.JobRepository
                .Received(1)
                .GetCountByProjectId(job.ProjectId);

            await sut.UnitOfWork.ProjectRepository
                .Received(1)
                .GetByIdUntracked(job.ProjectId);

            await sut.UnitOfWork.JobRepository
                .Received(1)
                .AddAsync(job);

            await sut.UnitOfWork
                .Received(1)
                .SaveChangesAsync();
        }

        [Theory]
        [CustomInlineAutoData(null)]
        [CustomInlineAutoData("")]
        [CustomInlineAutoData("   ")]
        public async Task AddAsync_WhenTitleIsInvalid_ShouldThrowException(
            string title,
            Job job,
            JobService sut)
        {
            job.Title = title;

            Func<Task> addAsync = async () => await sut.AddAsync(job);

            await addAsync
                .Should()
                .ThrowExactlyAsync<ContractViolationException>()
                .WithMessage(JobService.NULL_TITLE_MESSAGE);

            await sut.UnitOfWork.JobRepository
                .DidNotReceive()
                .GetCountByProjectId(Arg.Any<Guid>());
            
            await sut.UnitOfWork.ProjectRepository
                .DidNotReceive()
                .GetByIdUntracked(job.ProjectId);

            await sut.UnitOfWork.JobRepository
                .DidNotReceive()
                .AddAsync(job);

            await sut.UnitOfWork
                .DidNotReceive()
                .SaveChangesAsync();
        }

        [Theory, CustomAutoData]
        public async Task AddAsync_WhenProjectDoesNotExist_ShouldThrowException(
            Job job,
            JobService sut)
        {
            sut.UnitOfWork.JobRepository
                .GetCountByProjectId(job.ProjectId)
                .Returns(7);

            sut.UnitOfWork.JobRepository
                .AddAsync(job)
                .ReturnsNull();

            Func<Task> addAsync = async () => await sut.AddAsync(job);

            await addAsync
                .Should()
                .ThrowExactlyAsync<ProjectNotFoundException>()
                .WithMessage(JobService.PROJECT_NOT_FOUND_MESSAGE);

            await sut.UnitOfWork.JobRepository
                .Received(1)
                .GetCountByProjectId(job.ProjectId);

            await sut.UnitOfWork.JobRepository
                .DidNotReceive()
                .AddAsync(job);

            await sut.UnitOfWork
                .DidNotReceive()
                .SaveChangesAsync();
        }

        [Theory, CustomAutoData]
        public async Task DeleteAsync_ShouldDeleteCorrectly(
            Guid jobId,
            JobService sut)
        {
            sut.UnitOfWork
                .SaveChangesAsync()
                .Returns(Task.CompletedTask);

            await sut.DeleteAsync(jobId);

            sut.UnitOfWork.JobRepository
                .Received(1)
                .Delete(Arg.Do<Job>(x =>
                {
                    x.Id
                    .Should()
                    .Be(jobId);
                }));

            await sut.UnitOfWork
                .Received(1)
                .SaveChangesAsync();
        }

        [Theory, CustomAutoData]
        public async Task UpdateAsync_ShouldUpdateCorrectly(
            Job newJob,
            Job originalJob,
            JobService sut)
        {
            var originalJobToLog = JsonConvert.SerializeObject(originalJob);

            sut.UnitOfWork.JobRepository
                .GetByIdAsync(newJob.Id)
                .Returns(originalJob);

            var before = new
            {
                originalJob.Description,
                originalJob.Status,
                originalJob.IsEnabled,
                originalJob.Title
            };

            var after = new
            {
                newJob.Description,
                newJob.Status,
                newJob.IsEnabled,
                newJob.Title
            };

            sut.UnitOfWork.JobEventRepository
                .AddAsync(Arg.Do<JobEvent>(x =>
                {
                    x.CreationDate
                    .Should()
                    .NotBe(default);

                    x.Description
                    .Should()
                    .Be(JsonConvert.SerializeObject(new
                    {
                        Before = before,
                        After = after
                    }));

                    x.JobId
                    .Should()
                    .Be(originalJob.Id);
                }))
                .Returns(Task.CompletedTask);

            sut.UnitOfWork
                .SaveChangesAsync()
                .Returns(Task.CompletedTask);

            await sut.UpdateAsync(newJob);

            originalJob.Description
                .Should()
                .Be(newJob.Description);

            originalJob.Status
                .Should()
                .Be(newJob.Status);

            originalJob.IsEnabled
                .Should()
                .Be(newJob.IsEnabled);

            originalJob.Title
                .Should()
                .Be(newJob.Title);

            await sut.UnitOfWork.JobRepository
                .Received(1)
                .GetByIdAsync(newJob.Id);

            await sut.UnitOfWork.JobEventRepository
                .Received(1)
                .AddAsync(Arg.Any<JobEvent>());

            await sut.UnitOfWork
                .Received(1)
                .SaveChangesAsync();
        }

        [Theory, CustomAutoData]
        public async Task UpdateAsync_WhenJobNotExist_ShouldThrowException(
            Job newJob,
            JobService sut)
        {
            sut.UnitOfWork.JobRepository
                .GetByIdAsync(newJob.Id)
                .ReturnsNull();

            Func<Task> updateAsync = async () => await sut.UpdateAsync(newJob);

            await updateAsync
                .Should()
                .ThrowExactlyAsync<JobNotFoundException>()
                .WithMessage(JobService.JOB_NOT_FOUND_MESSAGE);

            await sut.UnitOfWork.JobRepository
                .Received(1)
                .GetByIdAsync(newJob.Id);

            await sut.UnitOfWork.JobEventRepository
                .DidNotReceive()
                .AddAsync(Arg.Any<JobEvent>());

            await sut.UnitOfWork
                .DidNotReceive()
                .SaveChangesAsync();
        }

        [Theory, CustomAutoData]
        public async Task GetAllByProjectNameAsync_ShouldReturnCorrectly(
            string projectName,
            List<Job> jobs,
            JobService sut)
        {
            sut.UnitOfWork.JobRepository
                .GetAllByProjectNameAsync(projectName)
                .Returns(jobs);

            var result = await sut.GetAllByProjectNameAsync(projectName);

            await sut.UnitOfWork.JobRepository
                .Received(1)
                .GetAllByProjectNameAsync(projectName);

            result
                .Should()
                .BeEquivalentTo(jobs);
        }
    }
}