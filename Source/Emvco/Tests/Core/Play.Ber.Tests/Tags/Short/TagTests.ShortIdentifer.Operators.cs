using Play.Ber.Identifiers;
using Play.Ber.Tests.TestData;

using Xunit;

namespace Play.Ber.Tests.Short
{
    public partial class TagTests
    {
        #region Instance Members

        [Fact]
        public void ShortIdentifier_WhenExplicitlyCastingToInt_CreatesIntWithCorrectValue()
        {
            Tag testValue = new(0b_00000001);
            uint sut = (uint) testValue;

            Assert.Equal((uint) 0x01, sut);
        }

        [Fact]
        public void RandomShortIdentifier_WhenExplicitlyCastingToInt_CreatesIntWithCorrectValue()
        {
            byte expectedValue = ShortIdentifierTestValueFactory.CreateByte(Tests.TagTests._Random);
            Tag testValue = new(expectedValue);

            byte sut = (byte) testValue;

            Assert.Equal(sut, expectedValue);
        }

        #endregion
    }
}