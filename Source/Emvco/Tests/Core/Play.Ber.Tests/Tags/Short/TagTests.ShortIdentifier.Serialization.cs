using Play.Ber.Identifiers;
using Play.Ber.Tests.TestData;
using Play.Core.Extensions;

using Xunit;

namespace Play.Ber.Tests.Short
{
    public partial class TagTests
    {
        #region Instance Members

        [Fact]
        public void RandomShortIdentifier_WhenSerializingInstance_CreatesByteWithCorrectValue()
        {
            byte expectedValue = ShortIdentifierTestValueFactory.CreateByte(Tests.TagTests._Random);
            Tag testValue = new(expectedValue);

            byte[] sut = testValue.Serialize();

            Assert.Equal(sut[0], expectedValue);
        }

        [Fact]
        public void RandomShortIdentifierComponentParts_WhenInitializingAndSerializingInstance_CreatesByteWithCorrectValue()
        {
            ClassTypes? expectedClassType = ShortIdentifierTestValueFactory.GetClassType(Tests.TagTests._Random);
            DataObjectTypes? expectedDataObjectType = ShortIdentifierTestValueFactory.GetDataObjectType(Tests.TagTests._Random);
            byte expectedTagNumber = ShortIdentifierTestValueFactory.GetTagNumber(Tests.TagTests._Random);

            byte initializationValue = (byte) ((byte) expectedClassType | (byte) expectedDataObjectType | expectedTagNumber);

            Tag testThing = new(initializationValue);
            byte sut = testThing.Serialize()[0];

            Assert.Equal(sut, initializationValue);
        }

        [Fact]
        public void ShortIdentifier_WhenSerializingInstance_CreatesByteWithCorrectValue()
        {
            byte expectedValue = 0b00000001;
            Tag testValue = new(expectedValue);

            byte sut = testValue.Serialize()[0];

            Assert.Equal(sut, expectedValue);
        }

        [Fact]
        public void ShortIdentifierComponentParts_WhenSerializingInstance_CreatesByteWithCorrectValue()
        {
            byte expectedValue = 0b00000001;
            Tag testValue = new(expectedValue);

            byte sut = testValue.Serialize()[0];

            Assert.Equal(sut, expectedValue);
        }

        [Fact]
        public void RandomShortTag_WhenSerializingInstance_CreatesByteWithCorrectValue()
        {
            byte expectedValue = ShortIdentifierTestValueFactory.CreateByte(Tests.TagTests._Random);
            Tag testValue = new(expectedValue.AsReadOnlySpan());

            byte[]? sut = testValue.Serialize();

            Assert.Equal(sut[0], expectedValue);
        }

        [Fact]
        public void ShortTag_WhenSerializingInstance_CreatesByteWithCorrectValue()
        {
            byte expectedValue = 0b00000001;
            Tag testValue = new(expectedValue.AsReadOnlySpan());

            byte[]? sut = testValue.Serialize();

            Assert.Equal(sut[0], expectedValue);
        }

        [Fact]
        public void ShortTagComponentParts_WhenSerializingInstance_CreatesByteWithCorrectValue()
        {
            byte expectedValue = 0b00000001;
            Tag testValue = new(expectedValue.AsReadOnlySpan());

            byte[]? sut = testValue.Serialize();

            Assert.Equal(sut[0], expectedValue);
        }

        #endregion
    }
}