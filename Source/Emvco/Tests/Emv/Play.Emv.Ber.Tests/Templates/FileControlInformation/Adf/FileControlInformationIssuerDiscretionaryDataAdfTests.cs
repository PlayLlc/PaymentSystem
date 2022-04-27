using System;

using AutoFixture;

using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv;

using Xunit;

namespace Play.Emv.Ber.Tests.Templates;

public class FileControlInformationIssuerDiscretionaryDataAdfTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;

    #endregion

    #region Constructor

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public FileControlInformationIssuerDiscretionaryDataAdfTests()
    {
        _Fixture = new EmvFixture().Create();
    }

    #endregion

    #region Instance Members

    [Fact]
    public void PrimitiveValue_EncodingTagLengthValue_ReturnsExpectedResult()
    {
        byte[] expected = FileControlInformationIssuerDiscretionaryDataAdfBuilder.RawTagLengthValue;
        FileControlInformationIssuerDiscretionaryDataAdf sut = _Fixture.Create<FileControlInformationIssuerDiscretionaryDataAdf>();
        byte[] actual = sut.EncodeTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void PrimitiveValue_EncodingValue_ReturnsExpectedResult()
    {
        byte[] expected = FileControlInformationIssuerDiscretionaryDataAdfBuilder.RawTagLengthValue;
        FileControlInformationIssuerDiscretionaryDataAdf sut = _Fixture.Create<FileControlInformationIssuerDiscretionaryDataAdf>();
        byte[] actual = sut.EncodeValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void PrimitiveValue_DecodingValue_ReturnsExpectedResult()
    {
        FileControlInformationIssuerDiscretionaryDataAdf expected = _Fixture.Create<FileControlInformationIssuerDiscretionaryDataAdf>();
        FileControlInformationIssuerDiscretionaryDataAdf actual =
            FileControlInformationIssuerDiscretionaryDataAdf.Decode(FileControlInformationIssuerDiscretionaryDataAdfBuilder.RawTagLengthValue.AsMemory());

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion
}