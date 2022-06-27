using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.Identifiers.Long;
using Play.Ber.InternalFactories;
using Play.Ber.Lengths;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Ber.Tests.BerCodec;

public class BerCodecTests : TestBase
{
    #region Instance Values

    private readonly Codecs.BerCodec _SystemUnderTest = new(BerCodecTestDataConfiguration.EmvBerCodecConfiguration);

    #endregion

    #region Instance Members

    #region DecodeTags

    [Fact]
    public void BerCodec_DecodeTags_CreatesExpectedTags()
    {
        ClassTypes expectedClass = ClassTypes.Private;
        DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

        byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);

        ReadOnlySpan<byte> input = stackalloc byte[] {leadingOctet, 14, 45, 37, 12, 14, 23, 2};

        Tag[] expectedTags = new Tag[] {new(new byte[] {leadingOctet, 14}), new(45), new(37), new(12), new(14), new(23), new(2)};
        Tag[] actualTags = _SystemUnderTest.DecodeTags(input);

        Assert.Equal(expectedTags, actualTags);
    }

    #endregion

    #endregion

    #region GetContentOctets

    [Fact]
    public void BerCodec_GetContentOctetsShortTagIdentifierTLV_ReturnsLastByte()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {12, 1, 15};

        //TLV : FirstTwoBytesRepresentTheTagAndLength
        // Tag is a Short Identifier: 12
        // TagLength is 34 : ShortLength.
        // GetByteCountTag + GetByteCountForTagLength = 1+1 = 2.
        byte[] actualContentOctets = _SystemUnderTest.GetContentOctets(input);
        byte[] expected = new byte[] {15};

        Assert.Equal(expected, actualContentOctets);
    }

    [Fact]
    public void BerCodec_GetContentOctetsShortTagIdentifierAndTagLength_ReturnsLastThreeBytes()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {16, 3, 15, 22, 37};

        // Tag is a Short Identifier: 12
        // TagLength is 3 : ShortLength.
        // GetByteCountTag + GetByteCountForTagLength = 1+1 = 2 bytes.
        byte[] actualContentOctets = _SystemUnderTest.GetContentOctets(input);
        byte[] expected = new byte[] {15, 22, 37};

        Assert.Equal(expected, actualContentOctets);
    }

    [Fact]
    public void BerCodec_GetContentOctetsLongTagIdentifierTLV_ReturnsExpectedOctetContents()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {31, 15, 3, 22, 37, 0};

        //TLV:
        //Tag: LongIdentifier : Tag has 2 bytes
        //Length: 3rd byte : 3
        //Content Octets: 22, 37, 0
        byte[] expected = new byte[] {22, 37, 0};
        byte[] actualContentOctets = _SystemUnderTest.GetContentOctets(input);

        Assert.Equal(expected, actualContentOctets);
    }

    [Fact]
    public void BerCodec_GetContentOctetsLongTagIdentifierTlvInputAsOnly3Bytes_ReturnsEmptyByteArray()
    {
        byte[] input = new byte[] {63, 15, 3};

        //TLV:
        //Tag: LongIdentifier : Tag has 2 bytes
        //Length: Short length 3rd byte : 3
        byte[] expected = new byte[0];

        Assert.Throws<ArgumentOutOfRangeException>(() => _SystemUnderTest.GetContentOctets(input));
    }

    [Fact]
    public void BerCodec_GetContentOctetsLongTagIdentifierLongTagLength_ThrowsArgumentOutofRangeException()
    {
        //TLV:
        //Tag: LongIdentifier
        //Length : Long Length 136 => 9 bytes
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[] {31, 12, 136, 63, 15, 3, 7};

            _SystemUnderTest.GetContentOctets(input);
        });
    }

    [Fact]
    public void BerCodec_GetContentOctetsLongTagIdentifierLongTagLength_ReturnsExpectedContentOctets()
    {
        Assert.Throws<BerParsingException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[]
            {
                31, 12, 136, 63, 15, 3, 7, 63, 15, 3,
                7, 63, 15, 3, 7
            };

            //TLV:
            //Tag: LongIdentifier
            //Length : 136 => 136 & ~128 =8 + 1 = 9 bytes 9 > MaxByteCountForLength.

            byte[] actualContentOctets = _SystemUnderTest.GetContentOctets(input);
        });
    }

    [Fact]
    public void BerCodec_GetContentOctetsLongTLV_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> input = stackalloc byte[]
        {
            31, 12, 8, 63, 15, 3, 7, 63, 15, 3,
            7
        };

        byte[] expected = new byte[] {63, 15, 3, 7, 63, 15, 3, 7};
        byte[] actualContentOctets = _SystemUnderTest.GetContentOctets(input);

        Assert.Equal(expected, actualContentOctets);
    }

    #endregion

    #region DecodeFirstTag

    [Fact]
    public void BerCodec_DecodeFirstTagShortIdentifier_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {15, 3, 7, 63};

        Tag expected = new(15);
        Tag actual = _SystemUnderTest.DecodeFirstTag(input);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_DecodeFirstTagLongIdentifier_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {26, 3, 7, 37};

        Tag expected = new(new byte[] {26});
        Tag actual = _SystemUnderTest.DecodeFirstTag(input);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_DecodeFirstTagLongIdentifierByteMaxValue_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {byte.MaxValue, 3, 7, 37};

        Tag expected = new(new byte[] {byte.MaxValue, 3});
        Tag actual = _SystemUnderTest.DecodeFirstTag(input);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_DecodeFirstTagLongIdentifier_BytesTwoAndThreeHaveBitsEightSet_ThrowsException()
    {
        Assert.Throws<BerParsingException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[] {byte.MaxValue, byte.MaxValue, byte.MaxValue, 7, 37};

            Tag firstTag = _SystemUnderTest.DecodeFirstTag(input);
        });
    }

    [Fact]
    public void BerCodec_DecodeFirstTagLongIdentifier2ndByteIs0_ReturnsExpectedTag()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {31, 0, 8, 37};

        Tag expected = new(new byte[] {31, 0});
        Tag actual = _SystemUnderTest.DecodeFirstTag(input);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_TagWithPrivateLeadingOctet_ReturnsExpectedDecodedTag()
    {
        ClassTypes expectedClass = ClassTypes.Private;
        DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

        byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);
        ReadOnlySpan<byte> input = stackalloc byte[] {leadingOctet, 45};

        Tag decodedTag = _SystemUnderTest.DecodeFirstTag(input);

        Assert.Equal(dataObjectType, decodedTag.GetDataObject());
    }

    [Fact]
    public void BerCodec_DecodeFirstTagLongIdentifierInputValueLengthOf1WithEightBitSet_BerParsingExceptionIsThrown()
    {
        //191 : 0b10111111
        Assert.Throws<BerParsingException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[] {0b10111111};

            Tag actual = _SystemUnderTest.DecodeFirstTag(input);
        });
    }

    #endregion

    #region DecodeFirstTagLength

    [Fact]
    public void BerCodec_DecodeFirstTLVShortIdentifier_ReturnsExpectedLength()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {26, 3, 7, 37};

        TagLength expected = new(new Tag(26), 3);
        TagLength actualTLV = _SystemUnderTest.DecodeFirstTagLength(input);

        Assert.Equal(expected, actualTLV);
    }

    [Fact]
    public void BerCodec_DecodeFirstTLVWithLongIdentifier_ReturnsExpectedLength()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {31, 14, 6, 7, 37};

        TagLength expected = new(new Tag(new byte[] {31, 14}), 6);
        TagLength actualTLV = _SystemUnderTest.DecodeFirstTagLength(input);

        Assert.Equal(expected, actualTLV);
    }

    [Fact]
    public void BerCodec_DecodeFirstTLVWithLongLength_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {31, 14, 6, 15, 37, 12, 14, 23, 2};

        TagLength expected = new(new Tag(new byte[] {31, 14}), 6);
        TagLength tlv = _SystemUnderTest.DecodeFirstTagLength(input);

        Assert.Equal(expected, tlv);
    }

    #endregion

    #region DecodeChildren

    #endregion

    #region DecodeTagLengthValue

    [Fact]
    public void BerCodec_DecodeTagLengthValue_InputWithoutIdentifierAndShortLength_DecodesExpectedTagLengthValue()
    {
        ClassTypes expectedClass = ClassTypes.Universal;
        DataObjectTypes dataObjectType = DataObjectTypes.Primitive;

        byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType);

        ReadOnlySpan<byte> input = stackalloc byte[] {leadingOctet, 6, 45, 37, 12, 14, 23, 2};

        TagLengthValue result = _SystemUnderTest.DecodeTagLengthValue(input);

        Tag expectedTag = new(leadingOctet);
        Assert.Equal(expectedTag, result.GetTag());

        Length expectedLength = new(6);
        Assert.Equal(expectedLength, result.GetLength());

        byte[] expectedContent = input.Slice(2, input.Length - 2).ToArray();
        Assert.Equal(expectedContent, result.EncodeValue());
    }

    [Fact]
    public void BerCodec_DecodeTagLengthValue_CorrectContentOctetsAreEncoded()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {114, 14, 45, 37, 12, 14, 23, 2};

        TagLengthValue result = _SystemUnderTest.DecodeTagLengthValue(input);

        byte[] encoded = result.EncodeValue();
        byte[] expected = input.Slice(2, input.Length - 2).ToArray();
        Assert.Equal(expected, encoded);
    }

    //[Fact]
    //public void BerCodec_DecodeTagLengthValueThenEncodeItAgain_SameResultIsCreated()
    //{
    //    ReadOnlySpan<byte> input = stackalloc byte[] { 114, 14, 45, 37, 12, 14, 23, 2 };

    //    TagLengthValue tlv = _SystemUnderTest.DecodeTagLengthValue(input);

    //    byte[] 
    //}

    #endregion
}