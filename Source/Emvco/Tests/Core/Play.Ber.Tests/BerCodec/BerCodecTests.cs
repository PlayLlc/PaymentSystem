using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

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

    #region DecodeTags

    [Fact]
    public void EncodedTagArray1_DecodeTags_CreatesExpectedTags()
    {
        ClassTypes expectedClass = ClassTypes.Private;
        DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

        byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);

        ReadOnlySpan<byte> input = stackalloc byte[] {leadingOctet, 14, 45, 37, 12, 14, 23, 2};

        Tag[] expected = {new(new byte[] {leadingOctet, 14}), new(45), new(37), new(12), new(14), new(23), new(2)};
        Tag[] actual = _SystemUnderTest.DecodeTags(input);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EncodedTagArray2_DecodeTags_CreatesExpectedTags()
    {
        byte[] testValue =
        {
            0x9F, 0x40, 0x9F, 02, 0x9F, 0x03, 0x9F, 0x26, 0x82, 0x5F,
            0x34
        };
        Tag[] expected =
        {
            new(new byte[] {0x9F, 0x40}), new(new byte[] {0x9F, 02}), new(new byte[] {0x9F, 0x03}), new(new byte[] {0x9F, 0x26}), new(new byte[] {0x82}),
            new(new byte[] {0x5F, 0x34})
        };

        Tag[] actual = _SystemUnderTest.DecodeTags(testValue);

        Assert.Equal(expected, actual);
    }

    #endregion

    #region GetContentOctets

    [Fact]
    public void BerCodec_GetContentOctetsShortTagIdentifierTlv_ReturnsLastByte()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {12, 1, 15};

        // Tag: 12
        // Length: 1
        // Content Octets: 15
        byte[] actualContentOctets = _SystemUnderTest.GetContentOctets(input);
        byte[] expected = {15};

        Assert.Equal(expected, actualContentOctets);
    }

    [Fact]
    public void BerCodec_GetContentOctetsShortTagIdentifierAndTagLength_ReturnsLastThreeBytes()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {16, 3, 15, 22, 37};

        // Tag: 16
        // Length: 3.
        // Content Octets: 15, 22, 37
        byte[] actualContentOctets = _SystemUnderTest.GetContentOctets(input);
        byte[] expected = {15, 22, 37};

        Assert.Equal(expected, actualContentOctets);
    }

    [Fact]
    public void BerCodec_GetContentOctetsLongTagIdentifierTlv_ReturnsExpectedOctetContents()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {31, 15, 3, 22, 37, 0};

        //Tag: 31, 15
        //Length: 3
        //Content Octets: 22, 37, 0
        byte[] expected = {22, 37, 0};
        byte[] actualContentOctets = _SystemUnderTest.GetContentOctets(input);

        Assert.Equal(expected, actualContentOctets);
    }

    [Fact]
    public void BerCodec_GetContentOctetsLongTagIdentifierTlvInputAsOnly3Bytes_ReturnsEmptyByteArray()
    {
        byte[] input = {63, 15, 3};

        //Tag: 63, 15
        //Length: 3
        //Content Octets: empty

        Assert.Throws<ArgumentOutOfRangeException>(() => _SystemUnderTest.GetContentOctets(input));
    }

    [Fact]
    public void InvalidLengthEncoding_InvokingGetContentOctets_ThrowsException()
    {
        Assert.Throws<BerParsingException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[]
            {
                31, 12, 136, 0, 0, 0, 0, 0, 0, 0,
                1, 63
            };

            //Tag: 31, 12
            //Length : 136, 63, 15, 3, 7, 63, 15, 3, 7 

            _SystemUnderTest.GetContentOctets(input);
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

        byte[] expected = {63, 15, 3, 7, 63, 15, 3, 7};
        byte[] actualContentOctets = _SystemUnderTest.GetContentOctets(input);

        Assert.Equal(expected, actualContentOctets);
    }

    #endregion

    #region DecodeFirstTag

    [Fact]
    public void EncodedArrayOfTags_InvokingDecodeFirstTag_ReturnsExpectedLongIdentifierTag()
    {
        byte[] testValue = new byte[]
        {
            0x9F, 0x40, 0x82, 0x9F, 0x02, 0x9F, 0x03, 0x9F, 0x26, 0x5F,
            0x34, 0x5F, 0x34
        };

        Tag expected = new(new byte[] {0x9F, 0x40});
        Tag actual = _SystemUnderTest.DecodeFirstTag(testValue);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EncodedArrayOfTags_InvokingDecodeFirstTag_ReturnsExpectedShortIdentifierTag()
    {
        byte[] testValue = new byte[]
        {
            0x82, 0x9F, 0x40, 0x9F, 0x02, 0x9F, 0x03, 0x9F, 0x26, 0x5F,
            0x34, 0x5F, 0x34
        };

        Tag expected = new(new byte[] {0x82});
        Tag actual = _SystemUnderTest.DecodeFirstTag(testValue);
        Assert.Equal(expected, actual);
    }

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
        ReadOnlySpan<byte> input = stackalloc byte[] {159, 64, 3, 7, 37};

        Tag expected = new(new byte[] {159, 64});
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
    public void EncodedTagThatIsOutOfRange_InvokingDecodeFirstTag_ThrowsException()
    {
        Assert.Throws<BerParsingException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[] {byte.MaxValue, byte.MaxValue, byte.MaxValue, 7};

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
    public void InvalidLongIdentifierTag_InvokingDecodeFirstTag_ThrowsException()
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
    public void InvalidLengthEncoding_InvokingDecodeFirstTagLength_ThrowsException()
    {
        //Tag: 31, 12
        //Length : Long Length 136 => 8 bytes
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[] {31, 12, 136, 63, 15, 3, 7};

            _SystemUnderTest.DecodeFirstTagLength(input);
        });
    }

    [Fact]
    public void BerCodec_DecodeFirstTlvShortIdentifier_ReturnsExpectedLength()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {26, 3, 7, 37, 1};

        TagLength expected = new(new Tag(26), 3);
        TagLength actual = _SystemUnderTest.DecodeFirstTagLength(input);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_DecodeFirstTlvWithLongIdentifier_ReturnsExpectedLength()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {31, 14, 6};

        TagLength expected = new(new Tag(new byte[] {31, 14}), 6);
        TagLength actual = _SystemUnderTest.DecodeFirstTagLength(input);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_DecodeFirstTlvWithLongLength_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {31, 14, 6, 15, 37, 12, 14, 23, 2};

        TagLength expected = new(new Tag(new byte[] {31, 14}), 6);
        TagLength actual = _SystemUnderTest.DecodeFirstTagLength(input);

        Assert.Equal(expected, actual);
    }

    #endregion

    #region DecodeTagLengthValue

    [Fact]
    public void EncodedTlvArray_InvokingDecodeTagLengthValue_ReturnsExpectedTag()
    {
        byte tagTestValue = (byte) ((byte) ClassTypes.Universal | (byte) DataObjectTypes.Primitive);

        ReadOnlySpan<byte> testValue = stackalloc byte[] {tagTestValue, 6, 45, 37, 12, 14, 23, 2};

        TagLengthValue sut = _SystemUnderTest.DecodeTagLengthValue(testValue);

        Tag expected = new(tagTestValue);
        Assert.Equal(expected, sut.GetTag());
    }

    [Fact]
    public void EncodedTlvArray_InvokingDecodeTagLengthValue_ReturnsExpectedLength()
    {
        byte tagTestValue = (byte) ((byte) ClassTypes.Universal | (byte) DataObjectTypes.Primitive);

        ReadOnlySpan<byte> testValue = stackalloc byte[] {tagTestValue, 6, 45, 37, 12, 14, 23, 2};

        TagLengthValue sut = _SystemUnderTest.DecodeTagLengthValue(testValue);

        Length expectedLength = new((uint) 6);
        Assert.Equal(expectedLength, sut.GetLength());
    }

    [Fact]
    public void EncodedTlvArray_InvokingDecodeTagLengthValue_ReturnsExpectedContentOctets()
    {
        byte tagTestValue = (byte) ((byte) ClassTypes.Universal | (byte) DataObjectTypes.Primitive);

        ReadOnlySpan<byte> testValue = stackalloc byte[] {tagTestValue, 6, 45, 37, 12, 14, 23, 2};

        TagLengthValue sut = _SystemUnderTest.DecodeTagLengthValue(testValue);

        byte[] expected = testValue.Slice(2, testValue.Length - 2).ToArray();
        byte[] actual = sut.EncodeValue();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_DecodeTagLengthValue_CorrectContentOctetsAreEncoded()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {114, 6, 45, 37, 12, 14, 23, 2};

        TagLengthValue result = _SystemUnderTest.DecodeTagLengthValue(input);

        byte[] expected = input.Slice(2, input.Length - 2).ToArray();
        byte[] actual = result.EncodeValue();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void InvalidEncodedLength_InvokingDecodeTagLengthValue_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[] {114, 14, 45, 37, 18, 14, 23, 16};

            TagLengthValue result = _SystemUnderTest.DecodeTagLengthValue(input);
        });
    }

    [Fact]
    public void EncodedTagLengthValue_DecodingThenEncoding_ReturnsExpectedResult()
    {
        byte[] testValue = new byte[] {87, 7, 32, 45, 37, 12, 14, 23, 2};

        TagLengthValue sut = _SystemUnderTest.DecodeTagLengthValue(testValue);

        byte[] expected = testValue;
        byte[] actual = sut.EncodeTagLengthValue();

        Assert.Equal(actual, expected);
    }

    #endregion

    #region DecodeChildren

    [Fact]
    public void BerCodec_DecodeChildrenInputHas2TLVChildren_ReturnsExpectedResult()
    {
        ReadOnlyMemory<byte> input = new byte[] {87, 7, 32, 2, 37, 12, 14, 1, 2};

        EncodedTlvSiblings sut = _SystemUnderTest.DecodeChildren(input);

        Tag firstChildTag = new(32);
        Tag secondChildTag = new(14);

        uint[] tags = sut.GetTags();
        Assert.Equal(tags[0], (uint) firstChildTag);
        Assert.Equal(tags[1], (uint) secondChildTag);

        Assert.Equal(sut.GetTag(32), firstChildTag);
        Assert.Equal(sut.GetTag(14), secondChildTag);
    }

    [Fact]
    public void BerCodec_DecodeChildrentInputHas2TLVChildren_ExceptionIsThrowWhenTagIsNotFound()
    {
        ReadOnlyMemory<byte> input = new byte[] { 87, 7, 32, 2, 37, 12, 14, 1, 2 };

        EncodedTlvSiblings sut = _SystemUnderTest.DecodeChildren(input);

        Assert.Throws<BerParsingException>(() =>
        {
            Tag notFound = sut.GetTag(16);
        });
    }

    [Fact]
    public void BerCodec_DecodeChildrenInputHas2TLVChildren_ReturnsCorrectSiblingCount()
    {
        ReadOnlyMemory<byte> input = new byte[] { 87, 7, 32, 2, 37, 12, 14, 1, 2 };

        EncodedTlvSiblings sut = _SystemUnderTest.DecodeChildren(input);

        Assert.Equal(2, sut.SiblingCount());
    }

    [Fact]
    public void BerCodec_DecodeChildrenInputHas2TLVChildren_ReturnsExpectedContentOctets()
    {
        ReadOnlyMemory<byte> input = new byte[] { 87, 7, 32, 2, 37, 12, 14, 1, 2 };

        EncodedTlvSiblings sut = _SystemUnderTest.DecodeChildren(input);

        Tag firstChildTag = new(32);
        ReadOnlySpan<byte> firstChildValueOctets = sut.GetValueOctetsOfSibling(firstChildTag);
        byte[] expectedValueOctets = { 37, 12 };
        Assert.Equal(expectedValueOctets, firstChildValueOctets.ToArray());

        Tag secondChildTag = new(14);
        ReadOnlySpan<byte> secondChildValueOctets = sut.GetValueOctetsOfSibling(secondChildTag);
        byte[] expectedSecondChildValueOctets = { 2 };
        Assert.Equal(expectedSecondChildValueOctets, secondChildValueOctets.ToArray());
    }

    [Fact]
    public void BerCodec_DecodeChildrenHas1TlVChildrenBasedOnLengthByte_ReturnsExpectedResult()
    {
        ReadOnlyMemory<byte> input = new byte[] {36, 8, 16, 9, 34, 16, 27, 86, 33, 09};

        EncodedTlvSiblings sut = _SystemUnderTest.DecodeChildren(input);

        uint[] tags = sut.GetTags();

        Tag expectedChild = new(16);
        Assert.Equal(tags[0], (uint) expectedChild);
    }

    [Fact]
    public void BerCodec_DecodeChildrenInputHas1TLVChildrenBasedOnLengthByte_ReturnsExpectedSiblingCount()
    {
        ReadOnlyMemory<byte> input = new byte[] { 36, 8, 16, 9, 34, 16, 27, 86, 33, 09 };

        EncodedTlvSiblings sut = _SystemUnderTest.DecodeChildren(input);

        Assert.Equal(1, sut.SiblingCount());
    }

    [Fact]
    public void BerCodec_DecodeChildrenTagLengthIsOutOfRange_ExceptionIsThrown()
    {
        ClassTypes expectedClass = ClassTypes.Application;
        DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

        byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType);

        ReadOnlyMemory<byte> input = new byte[] {leadingOctet, 12, 16, 9, 34, 16, 27, 86, 33, 09};

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            EncodedTlvSiblings result = _SystemUnderTest.DecodeChildren(input);
        });
    }

    #endregion

    #region DecodeSiblings

    [Fact]
    public void BerCodec_DecodeSiblingsInputHas1Sibling_ReturnsExpectedResult()
    {
        ReadOnlyMemory<byte> input = new byte[] {
            //Tag
            13,
            //Length
            5,
            //Content Octets
            2, 37, 12, 14, 2 };

        EncodedTlvSiblings sut = _SystemUnderTest.DecodeSiblings(input);

        Tag expected = new(13);
        Assert.Equal(expected, sut.GetTag(13));
    }

    [Fact]
    public void BerCodec_DecodeSiblingsInputHas1Sibling_ReturnsExpectedContentOctets()
    {
        ReadOnlyMemory<byte> input = new byte[] {
            //Tag
            13,
            //Length
            5,
            //Content Octets
            2, 37, 12, 14, 2 };

        EncodedTlvSiblings sut = _SystemUnderTest.DecodeSiblings(input);

        byte[] expected = { 2, 37, 12, 14, 2 };

        Tag sibling = sut.GetTag(13);
        Assert.Equal(expected, sut.GetValueOctetsOfSibling(sibling).ToArray());
    }

    [Fact]
    public void BerCodec_DecodeSiblingsInputHas2Siblings_ReturnsExpectedSiblingCount()
    {
        ReadOnlyMemory<byte> input = new byte[] {
            //Tag
            13,
            //Length
            2,
            //Content octets
            2, 37,
            //Tag
            28,
            //Length
            3,
            //Content octets
            12, 14, 2 };

        EncodedTlvSiblings sut = _SystemUnderTest.DecodeSiblings(input);

        Assert.Equal(2, sut.GetTags().Length);
    }

    [Fact]
    public void BerCodec_DecodeSiblingsInputHas2Siblings_ReturnsExpectedTags()
    {
        ReadOnlyMemory<byte> input = new byte[] {
            //Tag
            13,
            //Length
            2,
            //Content octets
            2, 37,
            //Tag
            28,
            //Length
            3,
            //Content octets
            12, 14, 2 };

        EncodedTlvSiblings sut = _SystemUnderTest.DecodeSiblings(input);

        Tag firstSibling = new(13);
        Assert.Equal(firstSibling, sut.GetTag(13));

        Tag secondSibling = new(28);
        Assert.Equal(secondSibling, sut.GetTag(28));
    }

    #endregion

    #region DecodeTagLengthValues

    [Fact]
    public void BerCodec_DecodeTagLengthValues_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {87, 1, 16, 32, 2, 37, 12, 14, 1, 2};

        TagLengthValue[] tlvs = _SystemUnderTest.DecodeTagLengthValues(input);

        Assert.Equal(3, tlvs.Length);

        Tag firstTag = new(87);
        Assert.Equal(firstTag, tlvs[0].GetTag());
        Assert.Equal(new byte[] {16}, tlvs[0].EncodeValue());

        Tag secondTag = new(32);
        Assert.Equal(secondTag, tlvs[1].GetTag());
        Assert.Equal(new byte[] {37, 12}, tlvs[1].EncodeValue());

        Tag thirdTag = new(14);
        Assert.Equal(thirdTag, tlvs[2].GetTag());
        Assert.Equal(new byte[] {2}, tlvs[2].EncodeValue());
    }

    [Fact]
    public void BerCodec_DecodeTagLengthValues_EncodingThemBackReturnsInput()
    {
        ClassTypes expectedClass = ClassTypes.Application;
        DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

        byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType);

        ReadOnlySpan<byte> input = stackalloc byte[]
        {
            leadingOctet, 2, 13, 16, 31, 14, 1, 27, 38, 2,
            28, 13
        };

        TagLengthValue[] tlvs = _SystemUnderTest.DecodeTagLengthValues(input);

        byte[] buffer = new byte[input.Length];
        int index = 0;

        foreach (TagLengthValue tlv in tlvs)
        {
            byte[] encodedTlv = tlv.EncodeTagLengthValue();
            encodedTlv.CopyTo(buffer, index);

            index += encodedTlv.Length;
        }

        Assert.Equal(buffer, input.ToArray());
    }

    [Fact]
    public void BerCodec_DecodeTagLengthValuesTLVLengthOutOfRange_ExceptionIsThrown()
    {
        //Second TLV: Tag: 31, 14 (LongIdentifierSet), Length: 12 -> OutOfRange
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[]
            {
                96, 2, 13, 16, 31, 14, 12, 27, 38, 2,
                28, 13
            };

            TagLengthValue[] tlvs = _SystemUnderTest.DecodeTagLengthValues(input);
        });
    }

    #endregion

    #region DecodeTagLengths

    [Fact]
    public void BerCodec_DecodeTagLengths_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {96, 12, 35, 14, 12, 8};

        TagLength[] tagLengths = _SystemUnderTest.DecodeTagLengths(input);

        Assert.Equal(3, tagLengths.Length);

        TagLength firstTl = new(new Tag(96), 12);
        Assert.Equal(firstTl, tagLengths[0]);

        TagLength secondTl = new(new Tag(35), 14);
        Assert.Equal(secondTl, tagLengths[1]);

        TagLength thirdTl = new(new Tag(12), 8);
        Assert.Equal(thirdTl, tagLengths[2]);
    }

    [Fact]
    public void BerCodec_DecodeTagLengthsWithLongIdentifiers_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> input = stackalloc byte[] {63, 18, 12, 31, 32, 14, 12, 8};

        TagLength[] tagLengths = _SystemUnderTest.DecodeTagLengths(input);

        Assert.Equal(3, tagLengths.Length);

        TagLength firstTl = new(new Tag(new byte[] {63, 18}), 12);
        Assert.Equal(firstTl, tagLengths[0]);

        TagLength secondTl = new(new Tag(new byte[] {31, 32}), 14);
        Assert.Equal(secondTl, tagLengths[1]);

        TagLength thirdTl = new(new Tag(12), 8);
        Assert.Equal(thirdTl, tagLengths[2]);
    }

    [Fact]
    public void EncodedTagLengthArray_DecodingTagLength_HasExpectedLength()
    {
        ReadOnlySpan<byte> input = stackalloc byte[]
        {
            // Tag: 0x9F12
            0x9F, 0x12,

            // Length: 0x8112
            0x81, 0xC9,

            // Tag: 0x5F1F
            0x5F, 0x1F,

            // Length: 0x82C001
            0x82, 0xC0, 0x01,

            // Tag 0x9F05
            0x9F, 0x05,

            // Length 0x8101
            0x81, 0xC2
        };

        TagLength[] tagLengths = _SystemUnderTest.DecodeTagLengths(input);

        Assert.Equal(3, tagLengths.Length);
    }

    [Fact]
    public void EncodedTagLengthArray_DecodingTagLength_HasExpectedValueInIndexPosition0()
    {
        ReadOnlySpan<byte> input = stackalloc byte[]
        {
            // Tag: 0x9F12
            0x9F, 0x12,

            // Length: 0x8112
            0x81, 0xC9,

            // Tag: 0x5F1F
            0x5F, 0x1F,

            // Length: 0x82C001
            0x82, 0xC0, 0x01,

            // Tag 0x9F05
            0x9F, 0x05,

            // Length 0x8101
            0x81, 0xC2
        };

        TagLength[] tagLengths = _SystemUnderTest.DecodeTagLengths(input);

        TagLength expected = new(new Tag(new byte[] {0x9F, 0x12}), Length.Parse(new byte[] {0x81, 0xC9}));
        TagLength actual = tagLengths[0];

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EncodedTagLengthArray_DecodingTagLength_HasExpectedValueInIndexPosition2()
    {
        ReadOnlySpan<byte> input = stackalloc byte[]
        {
            // Tag: 0x9F12
            0x9F, 0x12,

            // Length: 0x8112
            0x81, 0xC9,

            // Tag: 0x5F1F
            0x5F, 0x1F,

            // Length: 0x82C001
            0x82, 0xC0, 0x01,

            // Tag 0x9F05
            0x9F, 0x05,

            // Length 0x8101
            0x81, 0xC2
        };

        TagLength[] tagLengths = _SystemUnderTest.DecodeTagLengths(input);

        TagLength expected = new(new Tag(new byte[] {0x9F, 0x05}), Length.Parse(new byte[] {0x81, 0xC2}));
        TagLength actual = tagLengths[2];

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EncodedTagLengthArray_DecodingTagLength_HasExpectedValueInIndexPosition1()
    {
        ReadOnlySpan<byte> input = stackalloc byte[]
        {
            // Tag: 0x9F12
            0x9F, 0x12,

            // Length: 0x8112
            0x81, 0xC9,

            // Tag: 0x5F1F
            0x5F, 0x1F,

            // Length: 0x82C001
            0x82, 0xC0, 0x01,

            // Tag 0x9F05
            0x9F, 0x05,

            // Length 0x8101
            0x81, 0xC2
        };

        TagLength[] tagLengths = _SystemUnderTest.DecodeTagLengths(input);

        TagLength expected = new(new Tag(new byte[] {0x5F, 0x1F}), Length.Parse(new byte[] {0x82, 0xC0, 0x01}));
        TagLength actual = tagLengths[1];

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_DecodeTagLengthsOddInputMissingLastLengthByte_IndexOutOfRangeExceptionIsThrown()
    {
        Assert.Throws<IndexOutOfRangeException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[] {96, 12, 35, 14, 12};

            TagLength[] tagLengths = _SystemUnderTest.DecodeTagLengths(input);
        });
    }

    #endregion
}