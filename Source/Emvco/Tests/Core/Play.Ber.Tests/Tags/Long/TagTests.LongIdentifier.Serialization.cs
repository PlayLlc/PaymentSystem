using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Identifiers;
using Play.Core.Iso8825.Tests.Ber.TestData;

using Xunit;

namespace Play.Ber.Tests.Tags.__Temp
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