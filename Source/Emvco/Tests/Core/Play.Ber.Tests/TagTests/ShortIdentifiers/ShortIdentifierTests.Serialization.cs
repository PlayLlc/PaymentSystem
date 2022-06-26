using Play.Ber.Identifiers;
using Play.Ber.Identifiers.Short;
using Play.Ber.Tests.TestData;
using Play.Core.Iso8825.Tests.Ber.TestData;

using Xunit;

namespace Play.Core.Iso8825.Tests.Ber.TagTests.ShortIdentifiers
{
    public partial class TagFactoryTests
    {
        #region Instance Members

        [Fact]
        public void RandomShortIdentifier_WhenSerializingInstance_CreatesByteWithCorrectValue()
        {
            byte expectedValue = ShortIdentifierTestValueFactory.CreateByte(_Random);
            Tag testValue = new(expectedValue);

            byte[] sut = testValue.Serialize();

            Assert.Equal(sut[0], expectedValue);
        }

        [Fact]
        public void RandomShortIdentifierComponentParts_WhenInitializingAndSerializingInstance_CreatesByteWithCorrectValue()
        {
            ClassTypes? expectedClassType = ShortIdentifierTestValueFactory.GetClassType(_Random);
            DataObjectTypes? expectedDataObjectType = ShortIdentifierTestValueFactory.GetDataObjectType(_Random);
            byte expectedTagNumber = ShortIdentifierTestValueFactory.GetTagNumber(_Random);

            byte initializationValue = (byte) ((byte) expectedClassType | (byte) expectedDataObjectType | expectedTagNumber);

            Tag testThing = new(initializationValue);
            var sut = testThing.Serialize()[0];

            Assert.Equal(sut, initializationValue);
        }

        [Fact]
        public void ShortIdentifier_WhenSerializingInstance_CreatesByteWithCorrectValue()
        {
            byte expectedValue = 0b00000001;
            Tag testValue = new(expectedValue);

            var sut = testValue.Serialize()[0];

            Assert.Equal(sut, expectedValue);
        }

        [Fact]
        public void ShortIdentifierComponentParts_WhenSerializingInstance_CreatesByteWithCorrectValue()
        {
            byte expectedValue = 0b00000001;
            Tag testValue = new(expectedValue);

            var sut = testValue.Serialize()[0];

            Assert.Equal(sut, expectedValue);
        }

        #endregion
    }
}