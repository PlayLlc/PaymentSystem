using System;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Ber.Lengths;

namespace Play.Ber.DataObjects;

public record UnknownPrimitiveValue : PrimitiveValue
{
    #region Static Metadata

    public static readonly PlayEncodingId PrimitiveCodecIdentifier = default;

    #endregion

    #region Instance Values

    private readonly Length _Length;
    private readonly Tag _Tag;

    #endregion

    #region Constructor

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public UnknownPrimitiveValue(TagLength value)
    {
        _Tag = value.GetTag();
        _Length = value.GetLength();
    }

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public UnknownPrimitiveValue(Tag tag, Length length)
    {
        _Tag = new Tag(tag);
        _Length = new Length(length);
    }

    #endregion

    #region Instance Members

    public TagLengthValue AsTagLengthValue()
    {
        byte[] octetContents = new byte[_Length.GetContentLength()];

        return new TagLengthValue(GetTag(), octetContents);
    }

    public override PlayEncodingId GetBerEncodingId() => throw new NotImplementedException();
    public override Tag GetTag() => _Tag;
    public override ushort GetValueByteCount(BerCodec codec) => throw new NotImplementedException();

    /// <exception cref="BerException"></exception>
    public byte[] Encode()
    {
        Span<byte> buffer = stackalloc byte[_Length.GetContentLength()];
        buffer.Fill(0x00);

        return buffer.ToArray();
    }

    public byte[] Encode(int length)
    {
        Span<byte> buffer = stackalloc byte[length];
        buffer.Fill(0x00);

        return buffer.ToArray();
    }

    #endregion

    #region Serialization

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static PrimitiveValue Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="codec"></param>
    /// <returns></returns>
    /// <exception cref="BerException"></exception>
    /// <exception cref="BerInternalException"></exception>
    public static PrimitiveValue Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        Tag tag = new(value);
        Length length = Length.Parse(value[tag.GetByteCount()..]);

        return new UnknownPrimitiveValue(tag, length);
    }

    /// <exception cref="BerException"></exception>
    public override byte[] EncodeValue(BerCodec codec) => Encode();

    public override byte[] EncodeValue(BerCodec codec, int length) => Encode(length);
    public new byte[] EncodeTagLengthValue(BerCodec codec, int length) => throw new NotImplementedException();
    public new byte[] EncodeTagLengthValue(BerCodec codec) => throw new NotImplementedException();

    #endregion

    #region Equality

    public bool Equals(UnknownPrimitiveValue? x, UnknownPrimitiveValue? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(UnknownPrimitiveValue obj) => obj.GetHashCode();

    #endregion
}