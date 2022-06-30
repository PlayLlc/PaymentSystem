using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Ber.Tests.BerCodec;

public partial class BerCodecTests :  TestBase
{
    #region Decode To DecodedMetadata

    [Fact]
    public void BerCodecPrimitive_DecodeWithValidPlayEncodingIdForBinaryCodec_ReturnsExpectedResult()
    {
        //PlayEncodingId is used to point to which Codec to go in order to decode the value.
        PlayEncodingId binarEncodingId = BinaryCodec.EncodingId;
        //4byes -> uint
        ReadOnlySpan<byte> input = stackalloc byte[] { 32, 12, 16, 4 };

        DecodedMetadata metadata = _SystemUnderTest.Decode(binarEncodingId, input);
        Assert.NotNull(metadata);
        DecodedResult<uint> result = metadata.ToUInt32Result();
        uint expected = 537661444;
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void BerCodecPrimitive_DecodeWithValidPlayEncodingIdForAlphabeticCodec_InvalidByteNotRepresentingAChar_ReturnsExpectedResult()
    {
        PlayEncodingId alphabeticEncodingId = AlphabeticCodec.EncodingId;

        Assert.Throws<KeyNotFoundException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[] { 13, 22, 17 };
            DecodedMetadata metadata = _SystemUnderTest.Decode(alphabeticEncodingId, input);
        });
    }

    [Fact]
    public void BerCodec_DecodeWithValidPlayEncodingIdForAlphabeticCodec_ValidBytesRepresentingChars_ReturnsExpectedResult()
    {
        PlayEncodingId alphabeticEncodingId = AlphabeticCodec.EncodingId;

        ReadOnlySpan<byte> input = stackalloc byte[] { 0x50, 0x4c, 0x41, 0x59 };

        DecodedMetadata metadata = _SystemUnderTest.Decode(alphabeticEncodingId, input);
        DecodedResult<char[]> result = (DecodedResult<char[]>)metadata;

        char[] expected = new char[] { 'P', 'L', 'A', 'Y' };
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void BerCodec_DecodeWithValidPlayEncodingIdForAlphaNumericCodec_ReturnsExpectedResult()
    {
        PlayEncodingId alphanumericCodecEncondingId = AlphaNumericCodec.EncodingId;

        ReadOnlySpan<byte> input = stackalloc byte[] { 0x50, 0x4c, 0x41, 0x59, 0x30, 0x31 };

        DecodedMetadata metadata = _SystemUnderTest.Decode(alphanumericCodecEncondingId, input);
        DecodedResult<char[]> result = (DecodedResult<char[]>)metadata;

        char[] expected = new char[] { 'P', 'L', 'A', 'Y', '0', '1' };
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void BerCodec_DecodeWithValidPlayEncodingIdForAlphaNumericCodec_InvalidByte_ExceptionIsThrown()
    {
        PlayEncodingId alphanumericCodecEncondingId = AlphaNumericCodec.EncodingId;

        Assert.Throws<KeyNotFoundException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[] { 12, 13 };

            DecodedMetadata metadata = _SystemUnderTest.Decode(alphanumericCodecEncondingId, input);
        });
    }

    [Fact]
    public void BerCodec_DecodeWithValidPlayEncodingIdForAlphaNumericSpecialCodec_ReturnsExpectedResult()
    {
        PlayEncodingId alphaNumericSpecialCodecEncodingId = AlphaNumericSpecialCodec.EncodingId;

        ReadOnlySpan<byte> input = stackalloc byte[] { 0x23, 0x5b, 0x50, 0x4c, 0x41, 0x59, 0x30, 0x31, 0x5d, 0x23 };

        DecodedMetadata metadata = _SystemUnderTest.Decode(alphaNumericSpecialCodecEncodingId, input);
        DecodedResult<char[]> result = (DecodedResult<char[]>)metadata;

        char[] expected = new char[] { '#', '[', 'P', 'L', 'A', 'Y', '0', '1', ']', '#' };
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void BerCodec_DecodeWithValidPlayEncodingIdForCompressedNumericCodec_InvalidByte_ReturnsExpectedResult()
    {
        PlayEncodingId compressedNumericEncodingId = CompressedNumericCodec.EncodingId;

        Assert.Throws<CodecParsingException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[] { 12, 13, 5 };

            DecodedMetadata metadata = _SystemUnderTest.Decode(compressedNumericEncodingId, input);
        });
    }

    [Fact]
    public void BerCodec_DecodeWithValidPlayEncodingIdForCompressedNumericCodec_ReturnsExpectedResult()
    {
        PlayEncodingId compressedNumericEncodingId = CompressedNumericCodec.EncodingId;

        ReadOnlySpan<byte> input = stackalloc byte[] { 48, 49, 50 };

        DecodedMetadata metadata = _SystemUnderTest.Decode(compressedNumericEncodingId, input);

        DecodedResult<uint> result = (DecodedResult<uint>)metadata;
        uint expected = 303132;

        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void BerCodec_DecodeWithValidPlayEncodingIdForNumericCodec_ExpectUShort()
    {
        PlayEncodingId numericCodecEncodingId = NumericCodec.EncodingId;

        ReadOnlySpan<byte> input = stackalloc byte[] { 12, 16 };

        DecodedMetadata metadata = _SystemUnderTest.Decode(numericCodecEncodingId, input);

        DecodedResult<ushort> result = metadata.ToUInt16Result();
        ushort expected = 1216;

        Assert.NotNull(result);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void BerCodec_DecodeWithValidPlayEncodingIdForNumericCodec_ExpectUInt()
    {
        PlayEncodingId numericCodecEncodingId = NumericCodec.EncodingId;

        ReadOnlySpan<byte> input = stackalloc byte[] { 12, 16, 8, 11 };

        DecodedMetadata metadata = _SystemUnderTest.Decode(numericCodecEncodingId, input);

        DecodedResult<uint> result = metadata.ToUInt32Result();
        uint expected = 12160811;

        Assert.NotNull(result);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void BerCodec_DecodeWithValidPlayEncodingIdForNumericCodec_ExpectULong()
    {
        PlayEncodingId numericCodecEncodingId = NumericCodec.EncodingId;

        ReadOnlySpan<byte> input = stackalloc byte[] { 12, 16, 8, 11, 7 };

        DecodedMetadata metadata = _SystemUnderTest.Decode(numericCodecEncodingId, input);

        DecodedResult<ulong> result = metadata.ToUInt64Result();
        ulong expected = 1216081107;

        Assert.NotNull(result);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void BerCodec_DecodeWithValidPlayEncodingIdForNumericCodec_ExpectBigInteger()
    {
        PlayEncodingId numericCodecEncodingId = NumericCodec.EncodingId;

        ReadOnlySpan<byte> input = stackalloc byte[] { 12, 16, 8, 11, 7, 22, 23, 34, 17 };
        DecodedMetadata metadata = _SystemUnderTest.Decode(numericCodecEncodingId, input);

        DecodedResult<BigInteger> result = metadata.ToBigInteger();
        BigInteger expected = 121608110722233417;

        Assert.NotNull(result);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void BerCodec_DecodeWithValidPlayEncodingIdForHexadecimalCodec_ExpectUshort()
    {
        PlayEncodingId hexadecimalCodecEncodingId = HexadecimalCodec.EncodingId;

        ReadOnlySpan<byte> input = stackalloc byte[] { 0x0c, 0x10 };
        DecodedMetadata metadata = _SystemUnderTest.Decode(hexadecimalCodecEncodingId, input);

        DecodedResult<ushort> result = metadata.ToUInt16Result();
        ushort expected = 3088;

        Assert.NotNull(result);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void BerCodec_DecodeWithValidPlayEncodingIdForHexadecimalCodec_ExpectULong()
    {
        PlayEncodingId hexadecimalCodecEncodingId = HexadecimalCodec.EncodingId;

        ReadOnlySpan<byte> input = stackalloc byte[] { 0x0c, 0x10, 0x0a, 0x05, 0x11, 0x05, 0x1e, 0x2c };
        DecodedMetadata metadata = _SystemUnderTest.Decode(hexadecimalCodecEncodingId, input);

        DecodedResult<ulong> result = metadata.ToUInt64Result();
        ulong expected = 869205744959168044;

        Assert.NotNull(result);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void BerCodec_DecodeWithInvalidPlayCodecEncodingId_ExceptionIsThrown()
    {
        PlayEncodingId invalidEncodingId = new(typeof(int));

        Assert.Throws<BerParsingException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[] { 12, 16 };
            DecodedMetadata metadata = _SystemUnderTest.Decode(invalidEncodingId, input);
        });
    }

    #endregion

    #region Byte Count

    [Fact]
    public void BerCodec_GetByteCountForAlphabeticCodecEncodingId_ReturnsExpectedResult()
    {
        PlayEncodingId alphabeticEncodingId = AlphabeticCodec.EncodingId;

        char[] input = { 'p', 'l', 'a', 'y' };

        ushort expected = 4;
        ushort actual = _SystemUnderTest.GetByteCount(alphabeticEncodingId, input);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_GetByteCountForAlphabeticCodecEncodingId_ReturnsExpectedResult2()
    {
        PlayEncodingId alphabeticEncodingId = AlphabeticCodec.EncodingId;

        char[] input = { '[', 'p', 'l', 'a', 'y' };

        ushort expected = 5;
        ushort actual = _SystemUnderTest.GetByteCount(alphabeticEncodingId, input);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_GetByteCountForAlphaNumericCodecEncodingId_ReturnsExpectedResult2()
    {
        PlayEncodingId alphaNumericCodecEncodingId = AlphaNumericCodec.EncodingId;

        char[] input = { 'p', 'l', 'a', 'y', '0', '1' };

        ushort expected = 6;
        ushort actual = _SystemUnderTest.GetByteCount(alphaNumericCodecEncodingId, input);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_GetByteCountForCompressedNumericCodecByteArray_ReturnsExpectedResult()
    {
        PlayEncodingId compressedNumericEncodingId = CompressedNumericCodec.EncodingId;

        byte[] input = new byte[] { 1, 2, 3, 4 };

        ushort actual = _SystemUnderTest.GetByteCount(compressedNumericEncodingId, input);
        ushort expected = 4;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_GetByteCountForCompressedNumericCodecCharArray_ReturnsExpectedResult()
    {
        PlayEncodingId compressedNumericEncodingId = CompressedNumericCodec.EncodingId;

        char[] input = new char[] { 'p', 'l', 'a', 'y' };

        ushort actual = _SystemUnderTest.GetByteCount(compressedNumericEncodingId, input);
        ushort expected = 2;

        Assert.Equal(expected, actual);
    }

    //NumericCodec
    [Fact]
    public void BerCodec_GetByteCountForNumericCodecByteArray_ReturnsExpectedResult()
    {
        PlayEncodingId numericCodecEncodingId = NumericCodec.EncodingId;

        byte[] input = new byte[] { 32, 13, 0x5f, 12, 11 };

        ushort actual = _SystemUnderTest.GetByteCount(numericCodecEncodingId, input);
        ushort expected = 5;

        Assert.Equal(expected, actual);
    }

    #endregion

    #region EncodeTagLengthValue Tag And Content Octets

    [Fact]
    public void BerCodec_EncodeTagLengthValueForGivenTagAndContentOctets_ReturnsExpectedResult()
    {
        Tag tag = new(12);

        ReadOnlySpan<byte> contentOctets = stackalloc byte[] { 34, 3, 4, 16 };
        byte[] expected = { 12, 4, 34, 3, 4, 16 };
        byte[] actual = _SystemUnderTest.EncodeTagLengthValue(tag, contentOctets);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_EncodeTagLengthValueForGivenLongIdentifierTagAndContentOctets_ReturnsExpectedResult()
    {
        Tag tag = new(new byte[] { 31, 12 });

        ReadOnlySpan<byte> contentOctets = stackalloc byte[] { 34, 5, 12, 4, 6, 12, 33 };
        byte[] expected = { 31, 12, 7, 34, 5, 12, 4, 6, 12, 33 };
        byte[] actual = _SystemUnderTest.EncodeTagLengthValue(tag, contentOctets);

        Assert.Equal(expected, actual);
    }

    #endregion
}
