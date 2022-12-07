using System;

using Play.Ber.Tags;
using Play.Ber.Tests.TestData;

using Xunit;

namespace Play.Ber.Tests.Long;

public partial class TagTests
{
    #region Instance Members

    [Theory]
    [MemberData(nameof(TagTestValues.GetValidTags), MemberType = typeof(TagTestValues))]
    public void ValidTagTestValue_ExplicitlyCastToInt_ReturnsCorrectValue(TagTestValue value)
    {
        Tag sut = new(value.Value.AsSpan());
        uint expected = value.GetUInt32();
        uint actual = sut;

        Assert.Equal(expected, actual);
    }

    #endregion
}