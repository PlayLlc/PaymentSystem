using System;

using Play.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Ber.Constructed;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests;

public class CommandTemplateTests : TestBase
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
        byte[] expected = new AmountAuthorizedNumericTestTlv().EncodeTagLengthValue();
        CommandTemplate sut = CommandTemplate.Decode(expected.AsSpan());
        byte[]? actual = sut.EncodeValue();

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
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