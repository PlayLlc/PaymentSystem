using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Identifiers;
using Play.Core.Iso8825.Tests.Ber.TagTests.Tags.TestData;

using Xunit;

namespace Play.Ber.Tests.Tags.__Temp
{
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
}