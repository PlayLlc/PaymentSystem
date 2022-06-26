using Play.Ber.Identifiers;
using Play.Ber.Identifiers.Short;
using Play.Ber.Tests.TestData;

using Xunit;

namespace Play.Core.Iso8825.Tests.Ber.TagTests.ShortIdentifiers
{
    public partial class TagFactoryTests
    {
        #region Instance Members

        [Fact]
        public void RandomShortIdentifier_WhenExplicitlyCastingToInt_CreatesIntWithCorrectValue()
        {
            byte expectedValue = ShortIdentifierTestValueFactory.CreateByte(_Random);
            Tag testValue = new(expectedValue);

            byte sut = (byte) testValue;

            Assert.Equal(sut, expectedValue);
        }

        [Fact]
        public void ShortIdentifier_WhenExplicitlyCastingToInt_CreatesIntWithCorrectValue()
        {
            Tag testValue = new(0b_00000001);
            uint sut = (uint) testValue;

            Assert.Equal((uint) 0x01, sut);
        }

        [Fact]
        public void ShortIdentifier_WhenExplicitlyCastingToTag_CreatesTagWithCorrectInstanceValues()
        {
            Tag testValue = new(0b_00000001);
            Tag sut = (Tag) testValue;

            Assert.Equal(0x01, sut.GetTagNumber());
            Assert.Equal(ClassTypes.Universal, sut.GetClass());
            Assert.Equal(DataObjectTypes.Primitive, sut.GetDataObject());
            Assert.Equal(1, sut.GetByteCount());
        }

        #endregion
    }
}