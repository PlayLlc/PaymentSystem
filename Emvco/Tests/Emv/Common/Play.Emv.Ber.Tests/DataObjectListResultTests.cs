using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Emv.TestData.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests;

public class DataObjectListResultTests
{
    #region Instance Members

    /// <summary>
    /// BerEncodingTagLengths_Deserializing_CreatesDataObjectList
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncodingTagLengths_Deserializing_CreatesDataObjectList()
    {
        PrimitiveValue testData = new ApplicationExpirationDateTestTlv().AsPrimitiveValue();
        DataObjectListResult testValue = new(testData);
        Assert.NotNull(testValue);
    }

    /// <summary>
    ///     BerEncodingTagLength_Encoding_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncodingTagLength_Encoding_ReturnsExpectedResult()
    {
        ApplicationExpirationDateTestTlv testData = new();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        DataObjectListResult sut = new(testData.AsPrimitiveValue());
        byte[] testValue = sut.AsByteArray();
        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     BerEncodingTagLengths_Encoding_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncodingTagLengths_Encoding_ReturnsExpectedResult()
    {
        List<TestTlv> buffer = new();

        buffer.Add(new ApplicationExpirationDateTestTlv());
        buffer.Add(new KernelIdentifierTestTlv());
        buffer.Add(new TransactionDateTestTlv());

        IEnumerable<byte> expectedResult = buffer.SelectMany(a => a.EncodeTagLengthValue());

        DataObjectListResult sut = new(buffer.Select(a => a.AsPrimitiveValue()).ToArray());
        byte[] testValue = sut.AsByteArray();
        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     BerEncodingTagLengths_InvokingAsCommandTemplate_CreatesCommandTemplate
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncodingTagLengths_InvokingAsCommandTemplate_CreatesCommandTemplate()
    {
        PrimitiveValue testData = new ApplicationExpirationDateTestTlv().AsPrimitiveValue();
        CommandTemplate testValue = new DataObjectListResult(testData).AsCommandTemplate();
        Assert.NotNull(testValue);
    }

    /// <summary>
    ///     BerEncodingTagLengths_InvokingAsCommandTemplate_CreatesExpectedCommandTemplate
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncodingTagLengths_InvokingAsCommandTemplate_CreatesExpectedCommandTemplate()
    {
        PrimitiveValue testData = new ApplicationExpirationDateTestTlv().AsPrimitiveValue();
        CommandTemplate expectedValue = new(testData.EncodeTagLengthValue(EmvCodec.GetBerCodec()).AsSpan());
        CommandTemplate testValue = new DataObjectListResult(testData).AsCommandTemplate();

        Assert.Equal(expectedValue, testValue);
    }

    #endregion
}