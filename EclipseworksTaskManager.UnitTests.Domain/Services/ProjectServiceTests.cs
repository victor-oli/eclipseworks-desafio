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
    public class ProjectServiceTests
    {
        [Theory, CustomAutoData]
        public void Sut_ShouldGuardItsClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ProjectService).GetConstructors());
        }

        [Theory, CustomAutoData]
        public void Delete_WhenProjectHasPendingJobs_ShouldThrowException(
            Guid id,
            ProjectService sut)
        {
            // arrange
            sut.UnitOfWork.ProjectRepository
                .CheckForPendingJobs(id)
                .Returns(true);

            // act
            Func<Task> delete = async () => await sut.Delete(id);

            // assert
            delete
                .Should()
                .ThrowExactlyAsync<PendingJobException>()
                .WithMessage("This Project cannot be deleted while there is any job pending. Please remove or finish the jobs before trying again.");

            sut.UnitOfWork.ProjectRepository
                .Received(1)
                .CheckForPendingJobs(id);

            sut.UnitOfWork.ProjectRepository
                .DidNotReceive()
                .Delete(Arg.Any<Guid>());
        }

        [Theory, CustomAutoData]
        public async Task Delete_WhenProjectHasNoPendingJobs_ShouldExecuteCorrectly(
            Guid id,
            ProjectService sut)
        {
            // arrange
            sut.UnitOfWork.ProjectRepository
                .CheckForPendingJobs(id)
                .Returns(false);

            // act
            await sut.Delete(id);

            // assert
            await sut.UnitOfWork.ProjectRepository
                .Received(1)
                .CheckForPendingJobs(id);

            sut.UnitOfWork.ProjectRepository
                .Received(1)
                .Delete(id);
        }

        [Theory, CustomAutoData]
        public async Task AddAsync_WhenProjectHaveMoreThenTwentyJobs_ShouldThrowException(
            Project project,
            ProjectService sut)
        {
            // arrange
            for (int i = 0; i < 21; i++)
                project.Jobs.Add(new Job());

            // act
            Func<Task> addAsync = async () => await sut.AddAsync(project);

            // assert
            await addAsync
                .Should()
                .ThrowExactlyAsync<JobsOffLimitException>()
                .WithMessage("Currently a project cannot have more than twenty jobs. Please consider this.");

            await sut.UnitOfWork.ProjectRepository
                .DidNotReceive()
                .AddAsync(project);

            await sut.UnitOfWork
                .DidNotReceive()
                .SaveChangesAsync();
        }

        [Theory, CustomAutoData]
        public async Task AddAsync_WhenProjctHaveLessThenTwentyJobs_ShouldAddCorrectly(
            Project project,
            ProjectService sut)
        {
            project.Jobs.Clear();

            sut.UnitOfWork.ProjectRepository
                .AddAsync(project)
                .Returns(Task.CompletedTask);

            sut.UnitOfWork
                .SaveChangesAsync()
                .Returns(Task.CompletedTask);

            await sut.AddAsync(project);

            await sut.UnitOfWork.ProjectRepository
                .Received(1)
                .AddAsync(project);

            await sut.UnitOfWork
                .Received(1)
                .SaveChangesAsync();
        }

        [Theory, CustomAutoData]
        public async Task GetAllByUserAsync_ShouldReturnCorrectly(
            string userName,
            List<Project> projects,
            ProjectService sut)
        {
            sut.UnitOfWork.ProjectRepository
                .GetAllByUserAsync(userName)
                .Returns(projects);

            var result = await sut.GetAllByUserAsync(userName);

            result
                .Should()
                .BeEquivalentTo(projects);

            await sut.UnitOfWork.ProjectRepository
                .Received(1)
                .GetAllByUserAsync(userName);
        }
    }
}