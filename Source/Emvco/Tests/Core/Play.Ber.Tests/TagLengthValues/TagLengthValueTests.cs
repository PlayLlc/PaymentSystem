using System;

using Play.Ber.Identifiers;
using Play.Testing.BaseTestClasses;
using TagLengthValue = Play.Ber.DataObjects.TagLengthValue;

using Xunit;
using Play.Ber.Tests.TestData;
using System.Linq;
using Play.Ber.Lengths;

namespace Play.Ber.Tests.TagLengthValues;

public class TagLengthValueTests : TestBase
{
    #region Instance Members

    public static readonly Random _Random = new();

    [Fact]
    public void TagLengthValue_InstantiateFromTag_InstantiateCorrectTLV()
    {
        Tag tag = new(12);
        ReadOnlySpan<byte> contentOctes = stackalloc byte[] { 13, 7, 36, 63, 0xF0 };

        TagLengthValue sut = new TagLengthValue(tag, contentOctes);

        Tag actual = sut.GetTag();
        Assert.Equal(tag, actual);
    }

    [Fact]
    public void TagLengthValue_InstantiateFromTagWithLongIdentifier_InstantiateCorrectTLV()
    {
        Tag tag = new(new byte[] { 31, 12 });
        ReadOnlySpan<byte> contentOctes = stackalloc byte[] { 0xF0, 0x05, 0x12, 0x1C };

        TagLengthValue sut = new TagLengthValue(tag, contentOctes);

        Tag actual = sut.GetTag();
        Assert.Equal(tag, actual);
    }

    [Fact]
    public void TagLengthValue_InstantiateFromRandomTagAndContentOctets_CreatesExpectedTLV()
    {
        Tag expectedtag = ShortIdentifierTestValueFactory.CreateTag(_Random);
        byte[] contentOctets = ByteArrayFactory.GetRandom(_Random, 50, byte.MinValue, byte.MaxValue).ToArray();

        TagLengthValue sut = new TagLengthValue(expectedtag, contentOctets);

        Tag actual = sut.GetTag();
        Assert.Equal(expectedtag, actual);
    }

    #endregion

    #region GetTagLengthValueByteCount

    [Fact]
    public void TagLengthValue_GetTagLengthValueByteCountForTLV_ReturnsExpectedResult()
    {
        Tag tag = new(28);
        byte[] contentOctets = new byte[] { 32, 17, 6, 12, 8, 11, 114 };

        TagLengthValue sut = new TagLengthValue(tag, contentOctets);

        uint expected = 9;
        uint actual = sut.GetTagLengthValueByteCount();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TagLengthValue_GetTagLengthValueByteCountForTLVWithLogIdentifiedTag_ReturnsExpectedResult()
    {
        Tag tag = new(new byte[] { 31, 48 });
        byte[] contentOctets = new byte[] { 1, 4, 43, 32, 23, 119 };

        TagLengthValue sut = new TagLengthValue(tag, contentOctets);

        uint expected = 9;
        uint actual = sut.GetTagLengthValueByteCount();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RandomTagLengthValue_GetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        Tag tag = ShortIdentifierTestValueFactory.CreateTag(_Random);
        byte[] contentOctets = ByteArrayFactory.GetRandom(_Random, 50, byte.MinValue, byte.MaxValue).ToArray();

        TagLengthValue sut = new TagLengthValue(tag, contentOctets);

        uint expected = (uint)(tag.GetByteCount() + contentOctets.Length + 1);
        uint actual = sut.GetTagLengthValueByteCount();

        Assert.Equal(expected, actual);
    }

    #endregion

    #region  GetValueByteCount

    [Fact]
    public void RandomTagLengthValue_GetValueByteCount_ReturnsExpectedResult()
    {
        Tag tag = ShortIdentifierTestValueFactory.CreateTag(_Random);
        byte[] contentOctets = ByteArrayFactory.GetRandom(_Random, 50, byte.MinValue, byte.MaxValue).ToArray();

        TagLengthValue sut = new TagLengthValue(tag, contentOctets);

        ushort expected = (ushort)contentOctets.Length;
        ushort actual = sut.GetValueByteCount();

        Assert.Equal(expected, actual);
    }

    #endregion

    #region GetLength

    [Fact]
    public void TagLengthValue_GetLengthForTLV_ReturnsExpectedResult()
    {
        Tag tag = new(36);
        byte[] contentOctets = new byte[] { 11, 94, 136, 17, 8 };

        TagLengthValue sut = new TagLengthValue(tag, contentOctets);

        Length expected = new Length(5);
        Length actual = sut.GetLength();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RandomTagLengthValue_GetLength_ReturnsExpectedResult()
    {
        Tag tag = ShortIdentifierTestValueFactory.CreateTag(_Random);
        byte[] contentOctets = ByteArrayFactory.GetRandom(_Random, 50, byte.MinValue, byte.MaxValue).ToArray();

        TagLengthValue sut = new TagLengthValue(tag, contentOctets);

        Length expected = new Length((ushort)contentOctets.Length);
        Length actual = sut.GetLength();

        Assert.Equal(expected, actual);
    }

    #endregion

    #region EncodeValue

    [Fact]
    public void RandomTagLengthValue_EncodeValue_ReturnsSameContentOctets()
    {
        Tag tag = new(13);
        byte[] contentOctets = { 13, 194, 0x5C };

        TagLengthValue sut = new TagLengthValue(tag, contentOctets);

        byte[] expected = contentOctets;
        byte[] actual = sut.EncodeValue();

        Assert.Equal(expected, actual);
    }

    #endregion

    #region EncodeTagLengthValue

    [Fact]
    public void TagLengthValue_EncodeTheTLV_ReturnsExpectedResult()
    {
        Tag tag = new(31);

        byte[] contentOctets = { 18, 6, 22, 3, 4, 5, 0xc, 0x09 };

        TagLengthValue sut = new TagLengthValue(tag, contentOctets);

        byte[] expected = { 31, 8, 18, 6, 22, 3, 4, 5, 0xc, 0x09 };
        byte[] actual = sut.EncodeTagLengthValue();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RandomTLV_LengthIsSerializedAsContentOctetsLength_ReturnsExpectedEncodedTLVVAlue()
    {
        Tag tag = ShortIdentifierTestValueFactory.CreateTag(_Random);
        byte[] contentOctets = ByteArrayFactory.GetRandom(_Random, 50, byte.MinValue, byte.MaxValue).ToArray();

        TagLengthValue sut = new TagLengthValue(tag, contentOctets);

        Span<byte> buffer = stackalloc byte[contentOctets.Length + 2];

        tag.Serialize().CopyTo(buffer);
        buffer[tag.GetByteCount()] = (byte)contentOctets.Length;
        contentOctets.CopyTo(buffer[^contentOctets.Length..]);

        byte[] expected = buffer.ToArray();
        byte[] actual = sut.EncodeTagLengthValue();

        Assert.Equal(expected, actual);
    }

    #endregion

    #region Equals

    [Fact]
    public void GivenTwoTLVs_InstantiatedFromSameTagAndContentOctets_AreEqual()
    {
        Tag tag = new(13);
        byte[] contentOctets = new byte[] { 25, 64, 234, 23, 7, 134, 14 };

        TagLengthValue sut1 = new TagLengthValue(tag, contentOctets);
        TagLengthValue sut2 = new TagLengthValue(tag, contentOctets);

        Assert.NotSame(sut1, sut2);
        Assert.Equal(sut1, sut2);
        Assert.True(sut1.Equals(sut2));
    }

    [Fact]
    public void OneTlvAndOneOtherTypeObject_Comparing_AreNotEqual()
    {
        Tag tag = new(13);
        byte[] contentOctets = new byte[] { 25, 64, 234, 23, 7, 134, 14 };

        TagLengthValue sut1 = new TagLengthValue(tag, contentOctets);
        Tag objectToCompareTo = new(11);

        Assert.False(sut1.Equals(objectToCompareTo));
    }

    [Fact]
    public void CompareTlvWithNullValue_AreNotEqual()
    {
        Tag tag = new(13);
        byte[] contentOctets = new byte[] { 25, 64, 234, 23, 7, 134, 14 };

        TagLengthValue sut1 = new TagLengthValue(tag, contentOctets);

        Assert.False(sut1.Equals(null));
    }

    #endregion

    #region Operators

    [Fact]
    public void GivenTwoTLVs_InstantiatedFromSameTagAndContentOctets_AreEqualWhenComparingThem()
    {
        Tag tag = new(13);
        byte[] contentOctets = new byte[] { 25, 64, 234, 23, 7, 134, 14 };

        TagLengthValue sut1 = new TagLengthValue(tag, contentOctets);
        TagLengthValue sut2 = new TagLengthValue(tag, contentOctets);

        Assert.NotSame(sut1, sut2);
        Assert.True(sut1 == sut2);
    }

    [Fact]
    public void GivenTwoTLVs_InstantiatedFromDifferentTagAndContentOctets_AreNotEqualWhenComparingThem()
    {
        Tag tag = new(13);
        byte[] contentOctets = new byte[] { 25, 64, 234, 23, 7, 134, 14 };

        TagLengthValue sut1 = new TagLengthValue(tag, contentOctets);

        Tag tag2 = new(23);
        byte[] contentOctets2 = new byte[] { 33, 31, 118, 23, 7, 198, 14 };
        TagLengthValue sut2 = new TagLengthValue(tag2, contentOctets2);

        Assert.NotSame(sut1, sut2);
        Assert.False(sut1 == sut2);
        Assert.True(sut1 != sut2);
    }

    #endregion
}
