using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.ValueTypes;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class PosCardholderInteractionInformationTests : TestBase
{
    #region Instance Members

    private static readonly byte[] _DefaultContents = { 3, 5, 3 };

    #endregion

    /// <summary>
    ///     BerEncoding_DeserializingDataElement_CreatesPrimitiveValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        PosCardholderInteractionInformation testValue = PosCardholderInteractionInformation.Decode(_DefaultContents.AsSpan());
        Assert.NotNull(testValue);
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElement_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        PosCardholderInteractionInformation sut = PosCardholderInteractionInformation.Decode(_DefaultContents.AsSpan());
        byte[] actual = sut.EncodeValue();

        Assert.Equal(_DefaultContents, actual);
    }

    ///// <summary>
    /////     BerEncoding_EncodingDataElementTlv_SerializesExpectedValue
    ///// </summary>
    ///// <exception cref="InvalidOperationException"></exception>
    ///// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        PosCardholderInteractionInformationTestTlv testData = new();
        PosCardholderInteractionInformation sut = PosCardholderInteractionInformation.Decode(_DefaultContents.AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    ///// <summary>
    /////     BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue
    ///// </summary>
    ///// <exception cref="InvalidOperationException"></exception>
    ///// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        PosCardholderInteractionInformationTestTlv testData = new();
        PosCardholderInteractionInformation sut = PosCardholderInteractionInformation.Decode(_DefaultContents.AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(PosCardholderInteractionInformation.Tag, testData.EncodeValue());
        Assert.Equal(testValue, expectedResult);
    }

    ///// <summary>
    /////     TagLengthValue_SerializingToBer_ReturnsExpectedResult
    ///// </summary>
    ///// <exception cref="InvalidOperationException"></exception>
    ///// <exception cref="BerParsingException"></exception>
    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        PosCardholderInteractionInformationTestTlv testData = new();
        PosCardholderInteractionInformation sut = PosCardholderInteractionInformation.Decode(_DefaultContents.AsSpan());

        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsSecondTapRequiredForWallet_ReturnsTrue()
    {
        PosCardholderInteractionInformationTestTlv testData = new();
        PosCardholderInteractionInformation sut = PosCardholderInteractionInformation.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsSecondTapRequiredForWallet());
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsSecondTapRequiredForWallet_ReturnsFalse()
    {
        uint input = 0b0001_1010_0101_0001_1111_0110;

        PosCardholderInteractionInformation sut = new PosCardholderInteractionInformation(input);

        Assert.False(sut.IsSecondTapRequiredForWallet());
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsOfflineDeviceCvmRequired_ReturnsTrue()
    {
        uint input = 0b0001_1010_0101_0001_1111_0110;

        PosCardholderInteractionInformation sut = new PosCardholderInteractionInformation(input);

        Assert.True(sut.IsOfflineDeviceCvmRequired());
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsOfflineDeviceCvmRequired_ReturnsFalse()
    {
        uint input = 0b0001_1010_0101_0110_1111_0110;

        PosCardholderInteractionInformation sut = new PosCardholderInteractionInformation(input);

        Assert.False(sut.IsOfflineDeviceCvmRequired());
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsAcknowledgementRequired_ReturnsTrue()
    {
        uint input = 0b0001_1010_0101_0110_1111_0110;

        PosCardholderInteractionInformation sut = new PosCardholderInteractionInformation(input);

        Assert.True(sut.IsAcknowledgementRequired());
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsAcknowledgementRequired_ReturnsFalse()
    {
        uint input = 0b0001_1010_0101_1100_1111_0110;

        PosCardholderInteractionInformation sut = new PosCardholderInteractionInformation(input);

        Assert.False(sut.IsAcknowledgementRequired());
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsOfflinePinChangeRequired_ReturnsTrue()
    {
        ReadOnlySpan<byte> encoded = stackalloc byte[] { 26, 92, 246 };

        PosCardholderInteractionInformation sut = PosCardholderInteractionInformation.Decode(encoded);

        Assert.True(sut.IsOfflinePinChangeRequired());
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsOfflinePinChangeRequired_ReturnsFalse()
    {
        ReadOnlySpan<byte> encoded = stackalloc byte[] { 26, 90, 246 };

        PosCardholderInteractionInformation sut = PosCardholderInteractionInformation.Decode(encoded);

        Assert.False(sut.IsOfflinePinChangeRequired());
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsContextConflicting_ReturnsTrue()
    {
        ReadOnlySpan<byte> encoded = stackalloc byte[] { 26, 90, 246 };

        PosCardholderInteractionInformation sut = PosCardholderInteractionInformation.Decode(encoded);

        Assert.True(sut.IsContextConflicting());
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsContextConflicting_ReturnsFalse()
    {
        ReadOnlySpan<byte> encoded = stackalloc byte[] { 26, 82, 246 };

        PosCardholderInteractionInformation sut = PosCardholderInteractionInformation.Decode(encoded);

        Assert.False(sut.IsContextConflicting());
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsOfflineDeviceCvmVerificationSuccessful_ReturnsTrue()
    {
        uint input = 0b0001_1010_0101_0001_1111_0110;

        PosCardholderInteractionInformation sut = new PosCardholderInteractionInformation(input);

        Assert.True(sut.IsOfflineDeviceCvmVerificationSuccessful());
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsOfflineDeviceCvmVerificationSuccessful_ReturnsFalse()
    {
        uint input = 0b0001_1010_0101_0110_1111_0110;

        PosCardholderInteractionInformation sut = new PosCardholderInteractionInformation(input);

        Assert.False(sut.IsOfflineDeviceCvmVerificationSuccessful());
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsSecondTapNeeded_ReturnsTrue()
    {
        ReadOnlySpan<byte> encoded = stackalloc byte[] { 26, 90, 246 };

        PosCardholderInteractionInformation sut = PosCardholderInteractionInformation.Decode(encoded);

        Assert.True(sut.IsSecondTapNeeded());
    }

    [Fact]
    public void PosCarholderInteractionInformation_IsSecondTapNeeded_ReturnsFalse()
    {
        uint value = 0b0110_110011110000;

        PosCardholderInteractionInformation sut = new PosCardholderInteractionInformation(value);

        Assert.False(sut.IsSecondTapNeeded());
    }

    [Fact]
    public void PosCarholderInteractionInformation_GetMaskedValueForMessageTableEntry_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> encoded = stackalloc byte[] { 2, 4, 6 };

        PosCardholderInteractionInformation sut = PosCardholderInteractionInformation.Decode(encoded);

        ReadOnlySpan<byte> messageTableInput = stackalloc byte[] { 1, 2, 3, 1, 2, 3, 4, 5};

        MessageTableEntry tableEntryValue = new MessageTableEntry(messageTableInput);

        uint expected = 0b0010_0000_0100_0000_0100;
        uint actual = sut.GetMaskedValue(tableEntryValue);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void PosCarholderInteractionInformation_Equals_ReturnsTrue()
    {
        PosCardholderInteractionInformationTestTlv testData = new();

        PosCardholderInteractionInformation sut1 = PosCardholderInteractionInformation.Decode(testData.EncodeValue().AsSpan());

        uint input = 0b00000011_00000101_00000011;
        PosCardholderInteractionInformation sut2 = new PosCardholderInteractionInformation(input);

        Assert.True(PosCardholderInteractionInformation.EqualsStatic(sut1, sut2));
    }
}
