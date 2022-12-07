using System;

using AutoFixture;

using Play.Ber.Exceptions;
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
    public void ConstructedValue_EncodingTagLengthValue_ReturnsExpectedResult()
    {
        byte[] expected = EmvFixture.FileControlInformationAdfBuilder.GetDefaultEncodedTagLengthValue();
        FileControlInformationAdf sut = _Fixture.Create<FileControlInformationAdf>();
        byte[] actual = sut.EncodeTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ConstructedValue_EncodingValue_ReturnsExpectedResult()
    {
        byte[] expected = EmvFixture.FileControlInformationAdfBuilder.GetDefaultEncodedValue();
        FileControlInformationAdf sut = _Fixture.Create<FileControlInformationAdf>();
        byte[] actual = sut.EncodeValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion
}