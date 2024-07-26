using AutoFixture.Idioms;
using EclipseworksTaskManager.Domain.Entities;
using EclipseworksTaskManager.UnitTests.Domain.AutoFixture;
using FluentAssertions;
using Xunit;

namespace EclipseworksTaskManager.UnitTests.Domain.Entities
{
    public class ProjectTests
    {
        [Theory, CustomAutoData]
        public void Sut_ShouldGuardItsClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Project).GetConstructors());
        }

        [Fact]
        public void CanHaveMoreJobs_WhenTheNumberOfJobsIsLessThanTwenty_ShouldReturnTrue()
        {
            // arrange
            var project = new Project();

            for (int i = 0; i < 19; i++)
                project.Jobs.Add(new Job());

            // act
            var result = project.CanHaveMoreJobs();

            // assert
            result
                .Should()
                .BeTrue();
        }

        [Fact]
        public void CanHaveMoreJobs_WhenTheNumberOfJobsIsMoreThanNineTeen_ShouldReturnFalse()
        {
            // arrange
            var project = new Project();

            for (int i = 0; i < 20; i++)
                project.Jobs.Add(new Job());

            // act
            var result = project.CanHaveMoreJobs();

            // assert
            result
                .Should()
                .BeFalse();
        }
    }
}