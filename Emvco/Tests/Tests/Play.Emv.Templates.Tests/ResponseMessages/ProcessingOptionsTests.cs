using Play.Ber.DataObjects;
using Play.Emv.Templates.ResponseMessages;
using Play.Emv.TestData.Ber.Constructed;

using Xunit;

namespace Play.Emv.Templates.Tests.ResponseMessages
{
    public class ProcessingOptionsTests
    {
        #region Instance Members

        [Fact]
        public void BerEncoding_DeserializingTemplate_CreatesConstructedValue()
        {
            ProcessingOptionsTestTlv testData = new();
            ProcessingOptions sut = ProcessingOptions.Decode(testData.EncodeTagLengthValue());
            Assert.NotNull(sut);
        }

        [Fact]
        public void BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements()
        {
            ProcessingOptionsTestTlv testData = new();
            ProcessingOptions sut = ProcessingOptions.Decode(testData.EncodeTagLengthValue());
            byte[] testValue = sut.EncodeTagLengthValue();
            byte[]? expectedResult = testData.EncodeTagLengthValue();
            Assert.Equal(expectedResult, testValue);
        }

        [Fact]
        public void Template_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
        {
            ProcessingOptionsTestTlv testData = new();
            ProcessingOptions sut = ProcessingOptions.Decode(testData.EncodeTagLengthValue());
            Assert.True(sut.GetTagLengthValueByteCount() == testData.GetTagLengthValueByteCount());
        }

        [Fact]
        public void Template_InvokingGetValueByteCount_ReturnsExpectedResult()
        {
            ProcessingOptionsTestTlv testData = new();
            ProcessingOptions sut = ProcessingOptions.Decode(testData.EncodeTagLengthValue());
            Assert.True(sut.GetValueByteCount() == testData.GetValueByteCount());
        }

        [Fact]
        public void Template_InvokingAsTagLengthValue_ReturnsExpectedResult()
        {
            ProcessingOptionsTestTlv testData = new();
            ProcessingOptions sut = ProcessingOptions.Decode(testData.EncodeTagLengthValue());
            TagLengthValue testValue = sut.AsTagLengthValue();
            TagLengthValue expectedResult = testData.AsTagLengthValue();
            Assert.Equal(expectedResult, testValue);
        }

        #endregion
    }
}