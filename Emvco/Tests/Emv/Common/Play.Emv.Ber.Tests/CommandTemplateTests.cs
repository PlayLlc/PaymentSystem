using System;

using Play.Ber.Exceptions;
using Play.Emv.Templates.Requests;
using Play.Emv.TestData.Ber.Constructed;
using Play.Emv.TestData.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests;

public class CommandTemplateTests
{
    #region Instance Members

    /// <summary>
    ///     NullBerEncoding_DeserializingCommandTemplate_CreatesCommandTemplate
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void NullBerEncoding_DeserializingCommandTemplate_CreatesCommandTemplate()
    {
        CommandTemplate testValue = CommandTemplate.Decode(Array.Empty<byte>().AsSpan());
        Assert.NotNull(testValue);
    }

    /// <summary>
    ///     PrimitiveBerEncoding_DeserializingCommandTemplate_CreatesCommandTemplate
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void PrimitiveBerEncoding_DeserializingCommandTemplate_CreatesCommandTemplate()
    {
        byte[] expectedResult = new AmountAuthorizedNumericTestTlv().EncodeTagLengthValue();
        CommandTemplate sut = CommandTemplate.Decode(expectedResult.AsSpan());
        byte[]? testValue = sut.EncodeValue();
        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     ConstructedBerEncoding_DeserializingCommandTemplate_CreatesCommandTemplate
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
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