using AutoFixture.Xunit2;
using Xunit;

namespace EclipseworksTaskManager.UnitTests.Domain.AutoFixture
{
    public class CustomInlineAutoData : CompositeDataAttribute
    {
        public CustomInlineAutoData(params object[] values) : 
            base(new InlineDataAttribute(values), new CustomAutoDataAttribute()) { }
    }
}