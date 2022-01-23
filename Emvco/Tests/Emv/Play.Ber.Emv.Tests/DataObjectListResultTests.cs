﻿using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Emv.DataObjects;
using Play.Emv.TestData.Ber.Primitive;

using Xunit;

namespace Play.Ber.Emv.Tests
{
    public class DataObjectListResultTests
    {
        #region Instance Members

        [Fact]
        public void BerEncodingTagLengths_Deserializing_CreatesDataObjectList()
        {
            TagLengthValue testData = new ApplicationExpirationDateTestTlv().AsTagLengthValue();
            DataObjectListResult testValue = new(testData);
            Assert.NotNull(testValue);
        }

        [Fact]
        public void BerEncodingTagLength_Encoding_ReturnsExpectedResult()
        {
            ApplicationExpirationDateTestTlv testData = new();
            byte[] expectedResult = testData.EncodeTagLengthValue();
            DataObjectListResult sut = new(testData.AsTagLengthValue());
            byte[] testValue = sut.AsByteArray();
            Assert.Equal(expectedResult, testValue);
        }

        [Fact]
        public void BerEncodingTagLengths_Encoding_ReturnsExpectedResult()
        {
            List<TestTlv> buffer = new();

            buffer.Add(new ApplicationExpirationDateTestTlv());
            buffer.Add(new KernelIdentifierTestTlv());
            buffer.Add(new TransactionDateTestTlv());

            IEnumerable<byte> expectedResult = buffer.SelectMany(a => a.EncodeTagLengthValue());

            DataObjectListResult sut = new(buffer.Select(a => a.AsTagLengthValue()).ToArray());
            byte[] testValue = sut.AsByteArray();
            Assert.Equal(expectedResult, testValue);
        }

        [Fact]
        public void BerEncodingTagLengths_InvokingAsCommandTemplate_CreatesCommandTemplate()
        {
            TagLengthValue testData = new ApplicationExpirationDateTestTlv().AsTagLengthValue();
            CommandTemplate testValue = new DataObjectListResult(testData).AsCommandTemplate();
            Assert.NotNull(testValue);
        }

        [Fact]
        public void BerEncodingTagLengths_InvokingAsCommandTemplate_CreatesExpectedCommandTemplate()
        {
            TagLengthValue testData = new ApplicationExpirationDateTestTlv().AsTagLengthValue();
            CommandTemplate expectedValue = new(testData.EncodeTagLengthValue());
            CommandTemplate testValue = new DataObjectListResult(testData).AsCommandTemplate();

            Assert.Equal(expectedValue, testValue);
        }

        #endregion
    }
}