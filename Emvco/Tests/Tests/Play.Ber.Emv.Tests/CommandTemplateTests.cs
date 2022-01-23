using System;

using Play.Ber.Emv.DataObjects;
using Play.Emv.TestData.Ber.Constructed;
using Play.Emv.TestData.Ber.Primitive;

using Xunit;

namespace Play.Ber.Emv.Tests
{
    public class CommandTemplateTests
    {
        #region Instance Members

        [Fact]
        public void NullBerEncoding_DeserializingCommandTemplate_CreatesCommandTemplate()
        {
            CommandTemplate testValue = CommandTemplate.Decode(Array.Empty<byte>().AsSpan());
            Assert.NotNull(testValue);
        }

        [Fact]
        public void PrimitiveBerEncoding_DeserializingCommandTemplate_CreatesCommandTemplate()
        {
            byte[] expectedResult = new AmountAuthorizedNumericTestTlv().EncodeTagLengthValue();
            CommandTemplate sut = CommandTemplate.Decode(expectedResult.AsSpan());
            byte[]? testValue = sut.EncodeValue();
            Assert.Equal(expectedResult, testValue);
        }

        [Fact]
        public void ConstructedBerEncoding_DeserializingCommandTemplate_CreatesCommandTemplate()
        {
            byte[] expectedResult = new DirectoryEntryTestTlv().EncodeTagLengthValue();
            CommandTemplate sut = CommandTemplate.Decode(expectedResult.AsSpan());
            byte[]? testValue = sut.EncodeValue();
            Assert.Equal(expectedResult, testValue);
        }

        #endregion
    }
}