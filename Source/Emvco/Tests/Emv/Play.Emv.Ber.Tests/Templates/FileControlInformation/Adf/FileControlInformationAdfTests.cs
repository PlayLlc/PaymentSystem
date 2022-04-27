using System;

using AutoFixture;

using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv;

using Xunit;

namespace Play.Emv.Ber.Tests.Templates;

public class FileControlInformationAdfTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;

    #endregion

    #region Constructor

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public FileControlInformationAdfTests()
    {
        _Fixture = new EmvFixture().Create();
    }

    #endregion

    #region Instance Members

    [Fact]
    public void PrimitiveValue_EncodingTagLengthValue_ReturnsExpectedResult()
    {
        byte[] expected = FileControlInformationAdfBuilder.RawTagLengthValue;
        FileControlInformationAdf sut = _Fixture.Create<FileControlInformationAdf>();
        byte[] actual = sut.EncodeTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void PrimitiveValue_EncodingValue_ReturnsExpectedResult()
    {
        byte[] expected = FileControlInformationAdfBuilder.RawValue;
        FileControlInformationAdf sut = _Fixture.Create<FileControlInformationAdf>();
        byte[] actual = sut.EncodeValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void PrimitiveValue_DecodingValue_ReturnsExpectedResult()
    {
        FileControlInformationAdf expected = _Fixture.Create<FileControlInformationAdf>();
        FileControlInformationAdf actual = FileControlInformationAdf.Decode(FileControlInformationAdfBuilder.RawTagLengthValue.AsMemory());

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion
}