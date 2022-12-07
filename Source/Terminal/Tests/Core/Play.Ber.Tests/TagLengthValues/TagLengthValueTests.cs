using System;

using Play.Testing.BaseTestClasses;

using TagLengthValue = Play.Ber.DataObjects.TagLengthValue;

using Xunit;

using Play.Ber.Tests.TestData;

using System.Linq;

using Play.Ber.Lengths;
using Play.Ber.Tags;

namespace Play.Ber.Tests.TagLengthValues;

public class TagLengthValueTests : TestBase
{
    #region Static Metadata

    public static readonly Random _Random = new();

    #endregion

    #region GetTag

    [Fact]
    public void TagLengthValue_InstantiateFromTag_InstantiateCorrectTlv()
    {
        Tag tag = new(0xC);
        ReadOnlySpan<byte> contentOctets = stackalloc byte[] {13, 7, 36, 63, 0xF0};

        TagLengthValue sut = new(tag, contentOctets);

        Tag actual = sut.GetTag();
        Assert.Equal(tag, actual);
    }

    [Fact]
    public void TagLengthValue_InstantiateFromTagWithLongIdentifier_InstantiateCorrectTlv()
    {
        Tag tag = new(new byte[] {0x1F, 0x0C});
        ReadOnlySpan<byte> contentOctets = stackalloc byte[] {0xF0, 0x05, 0x12, 0x1C};

        TagLengthValue sut = new(tag, contentOctets);

        Tag actual = sut.GetTag();
        Assert.Equal(tag, actual);
    }

    [Fact]
    public void TagLengthValue_InstantiateFromRandomTagAndContentOctets_CreatesExpectedTlv()
    {
        Tag expectedTag = ShortIdentifierTestValueFactory.CreateTag(_Random);
        byte[] contentOctets = ByteArrayFactory.GetRandom(_Random, 50, byte.MinValue, byte.MaxValue).ToArray();

        TagLengthValue sut = new(expectedTag, contentOctets);

        Tag actual = sut.GetTag();
        Assert.Equal(expectedTag, actual);
    }

    #region GetValueByteCount

    [Fact]
    public void RandomTagLengthValue_GetValueByteCount_ReturnsExpectedResult()
    {
        Tag tag = ShortIdentifierTestValueFactory.CreateTag(_Random);
        byte[] contentOctets = ByteArrayFactory.GetRandom(_Random, 50, byte.MinValue, byte.MaxValue).ToArray();

        TagLengthValue sut = new(tag, contentOctets);

        ushort expected = (ushort) contentOctets.Length;
        ushort actual = sut.GetValueByteCount();

        Assert.Equal(expected, actual);
    }

    #endregion

    #region EncodeValue

    [Fact]
    public void RandomTagLengthValue_EncodeValue_ReturnsSameContentOctets()
    {
        Tag tag = new(13);
        byte[] contentOctets = {13, 194, 0x5C};

        TagLengthValue sut = new(tag, contentOctets);

        byte[] expected = contentOctets;
        byte[] actual = sut.EncodeValue();

        Assert.Equal(expected, actual);
    }

    #endregion

    #endregion

    #region GetTagLengthValueByteCount

    [Fact]
    public void TagLengthValue_GetTagLengthValueByteCountForTlv_ReturnsExpectedResult()
    {
        Tag tag = new(28);
        byte[] contentOctets = {32, 17, 6, 12, 8, 11, 114};

        TagLengthValue sut = new(tag, contentOctets);

        uint expected = 9;
        uint actual = sut.GetTagLengthValueByteCount();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TagLengthValue_GetTagLengthValueByteCountForTlvWithLogIdentifiedTag_ReturnsExpectedResult()
    {
        Tag tag = new(new byte[] {31, 48});
        byte[] contentOctets = {1, 4, 43, 32, 23, 119};

        TagLengthValue sut = new(tag, contentOctets);

        uint expected = 9;
        uint actual = sut.GetTagLengthValueByteCount();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RandomTagLengthValue_GetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        Tag tag = ShortIdentifierTestValueFactory.CreateTag(_Random);
        byte[] contentOctets = ByteArrayFactory.GetRandom(_Random, 50, byte.MinValue, byte.MaxValue).ToArray();

        TagLengthValue sut = new(tag, contentOctets);

        uint expected = (uint) (tag.GetByteCount() + contentOctets.Length + 1);
        uint actual = sut.GetTagLengthValueByteCount();

        Assert.Equal(expected, actual);
    }

    #endregion

    #region GetLength

    [Fact]
    public void TagLengthValue_GetLengthForTlv_ReturnsExpectedResult()
    {
        Tag tag = new(36);
        byte[] contentOctets = {11, 94, 136, 17, 8};

        TagLengthValue sut = new(tag, contentOctets);

        Length expected = new(5);
        Length actual = sut.GetLength();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RandomTagLengthValue_GetLength_ReturnsExpectedResult()
    {
        Tag tag = ShortIdentifierTestValueFactory.CreateTag(_Random);
        byte[] contentOctets = ByteArrayFactory.GetRandom(_Random, 50, byte.MinValue, byte.MaxValue).ToArray();

        TagLengthValue sut = new(tag, contentOctets);

        Length expected = new((ushort) contentOctets.Length);
        Length actual = sut.GetLength();

        Assert.Equal(expected, actual);
    }

    #endregion

    #region EncodeTagLengthValue

    [Fact]
    public void TagLengthValue_EncodeTheTlv_ReturnsExpectedResult()
    {
        Tag tag = new(31);

        byte[] contentOctets = {18, 6, 22, 3, 4, 5, 0xc, 0x09};

        TagLengthValue sut = new(tag, contentOctets);

        byte[] expected = {31, 8, 18, 6, 22, 3, 4, 5, 0xc, 0x09};
        byte[] actual = sut.EncodeTagLengthValue();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RandomTlv_LengthIsSerializedAsContentOctetsLength_ReturnsExpectedEncodedTLVVAlue()
    {
        Tag tag = ShortIdentifierTestValueFactory.CreateTag(_Random);
        byte[] contentOctets = ByteArrayFactory.GetRandom(_Random, 50, byte.MinValue, byte.MaxValue).ToArray();

        TagLengthValue sut = new(tag, contentOctets);

        Span<byte> buffer = stackalloc byte[contentOctets.Length + 2];

        tag.Serialize().CopyTo(buffer);
        buffer[tag.GetByteCount()] = (byte) contentOctets.Length;
        contentOctets.CopyTo(buffer[^contentOctets.Length..]);

        byte[] expected = buffer.ToArray();
        byte[] actual = sut.EncodeTagLengthValue();

        Assert.Equal(expected, actual);
    }

    #endregion

    #region Equals

    [Fact]
    public void GivenTwoTlvObjects_InstantiatedFromSameTagAndContentOctets_AreEqual()
    {
        Tag tag = new(13);
        byte[] contentOctets = {25, 64, 234, 23, 7, 134, 14};

        TagLengthValue sut1 = new(tag, contentOctets);
        TagLengthValue sut2 = new(tag, contentOctets);

        Assert.NotSame(sut1, sut2);
        Assert.Equal(sut1, sut2);
        Assert.True(sut1.Equals(sut2));
    }

    [Fact]
    public void OneTlvAndOneOtherTypeObject_Comparing_AreNotEqual()
    {
        Tag tag = new(13);
        byte[] contentOctets = {25, 64, 234, 23, 7, 134, 14};

        TagLengthValue sut1 = new(tag, contentOctets);
        Tag objectToCompareTo = new(11);

        Assert.False(sut1.Equals(objectToCompareTo));
    }

    [Fact]
    public void CompareTlvWithNullValue_AreNotEqual()
    {
        Tag tag = new(13);
        byte[] contentOctets = {25, 64, 234, 23, 7, 134, 14};

        TagLengthValue sut1 = new(tag, contentOctets);

        Assert.False(sut1.Equals(null));
    }

    #endregion

    #region Operators

    [Fact]
    public void GivenTwoTlvObjects_InstantiatedFromSameTagAndContentOctets_AreEqualWhenComparingThem()
    {
        Tag tag = new(13);
        byte[] contentOctets = {25, 64, 234, 23, 7, 134, 14};

        TagLengthValue sut1 = new(tag, contentOctets);
        TagLengthValue sut2 = new(tag, contentOctets);

        Assert.NotSame(sut1, sut2);
        Assert.True(sut1 == sut2);
    }

    [Fact]
    public void GivenTwoTlvObjects_InstantiatedFromDifferentTagAndContentOctets_AreNotEqualWhenComparingThem()
    {
        Tag tag = new(13);
        byte[] contentOctets = {25, 64, 234, 23, 7, 134, 14};

        TagLengthValue sut1 = new(tag, contentOctets);

        Tag tag2 = new(23);
        byte[] contentOctets2 = {33, 31, 118, 23, 7, 198, 14};
        TagLengthValue sut2 = new(tag2, contentOctets2);

        Assert.NotSame(sut1, sut2);
        Assert.False(sut1 == sut2);
        Assert.True(sut1 != sut2);
    }

    #endregion
}