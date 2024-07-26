using AutoFixture.Idioms;
using EclipseworksTaskManager.Domain.Services;
using EclipseworksTaskManager.UnitTests.Domain.AutoFixture;
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
    }
}