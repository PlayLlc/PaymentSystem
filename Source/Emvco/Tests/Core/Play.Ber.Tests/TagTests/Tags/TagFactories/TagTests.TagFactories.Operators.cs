using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Ber.Tests.TestData;
using Play.Core.Extensions;

using Xunit;

namespace Play.Ber.Tests.TagTests.Tags.TagFactories
{
    public partial class TagTests
    {
        #region ShortIdentifiers

        [Fact]
        public void RandomShortIdentifier_WhenExplicitlyCastingToInt_CreatesIntWithCorrectValue()
        {
            byte expectedValue = ShortIdentifierTestValueFactory.CreateByte(_Random);
            Tag actualValue = new(expectedValue);

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void ShortIdentifier_WhenExplicitlyCastingToInt_CreatesIntWithCorrectValue()
        {
            uint expected = 0x01;
            Tag testData = new(((byte) 0b_00000001).AsReadOnlySpan());
            uint actual = (uint) testData;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShortIdentifier_WhenExplicitlyCastingToTag_CreatesTagWithCorrectInstanceValues()
        {
            Tag sut = new(((byte) 0b_00000001).AsReadOnlySpan());

            Assert.Equal(0x01, sut.GetTagNumber());
            Assert.Equal(ClassTypes.Universal, sut.GetClass());
            Assert.Equal(DataObjectTypes.Primitive, sut.GetDataObject());
            Assert.Equal(1, sut.GetByteCount());
        }

        #endregion
    }
}