using System;

using Play.Ber.Identifiers;
using Play.Ber.Tests.TestData;

using Xunit;

namespace Play.Ber.Tests.Long
{
    public partial class TagTests
    {
        #region Instance Members

        [Theory]
        [MemberData(nameof(TagLengthValueTestValues.GetValidTestValues), MemberType = typeof(TagLengthValueTestValues))]
        public void TagLengthValueTestValue_ExplicitlyCastToInt_ReturnsCorrectValue(TagLengthValueTestValue value)
        {
            Tag sut = new(value.Tag.AsSpan());
            Assert.Equal(value.Tag, sut.Serialize());
        }

        #endregion
    }
}