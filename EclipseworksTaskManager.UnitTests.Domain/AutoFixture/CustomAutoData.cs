using AutoFixture;
using AutoFixture.Xunit2;
using EclipseworksTaskManager.Domain.Interfaces;
using EclipseworksTaskManager.Domain.Interfaces.Repository;
using EclipseworksTaskManager.Domain.Interfaces.Service;
using NSubstitute;

namespace EclipseworksTaskManager.UnitTests.Domain.AutoFixture
{
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute() : base(FixtureFactory) { }

        public static IFixture FixtureFactory()
        {
            var fixture = new Fixture();

            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            fixture.Register(() => Substitute.For<IUnitOfWork>());
            fixture.Register(() => Substitute.For<IJobRepository>());
            fixture.Register(() => Substitute.For<IProjectRepository>());
            fixture.Register(() => Substitute.For<IJobEventRepository>());
            fixture.Register(() => Substitute.For<IUserService>());

            return fixture;
        }
    }
}