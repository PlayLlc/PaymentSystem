using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.Lengths;

namespace Play.Ber.DataObjects;

public class TagLengthValue : IEncodeBerDataObjects, IEquatable<TagLengthValue>, IEqualityComparer<TagLengthValue>
{
    #region Instance Values

    protected readonly byte[] _ContentOctets;
    protected readonly Tag _Tag;

    #endregion

    #region Constructor

    public TagLengthValue(Tag tag, ReadOnlySpan<byte> contentOctets)
    {
        _Tag = tag;
        _ContentOctets = contentOctets.ToArray();
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     The byte count of the Tag, Length and Value fields of this TLV object
    /// </summary>
    /// <returns></returns>
    public uint GetTagLengthValueByteCount() => checked((uint) (_Tag.GetByteCount() + GetLength().GetByteCount() + _ContentOctets.Length));

    public ushort GetValueByteCount() => checked((ushort) _ContentOctets.Length);
    public Length GetLength() => new((ushort) _ContentOctets.Length);
    public Tag GetTag() => _Tag;
    public uint GetTagLengthValueByteCount(BerCodec codec) => GetTagLengthValueByteCount();
    public ushort GetValueByteCount(BerCodec codec) => GetValueByteCount();
    public TagLengthValue AsTagLengthValue(BerCodec codec) => this;

    #endregion

    #region Serialization

    public byte[] EncodeValue() => _ContentOctets;

    /// <summary>
    ///     EncodeTagLengthValue
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public byte[] EncodeTagLengthValue()
    {
        Length length = GetLength();
        Span<byte> buffer = stackalloc byte[_Tag.GetByteCount() + length.GetByteCount() + _ContentOctets.Length];

        GetTag().Serialize().CopyTo(buffer);
        length.Serialize().CopyTo(buffer[_Tag.GetByteCount()..]);
        _ContentOctets.CopyTo(buffer[^_ContentOctets.Length..]);

        return buffer.ToArray();
    }

    public byte[] EncodeValue(BerCodec codec) => _ContentOctets;
    public byte[] EncodeTagLengthValue(BerCodec codec) => EncodeTagLengthValue();

    #endregion

    #region Equality

    public override bool Equals(object? other) => other is TagLengthValue tlv && Equals(tlv);

    public bool Equals(TagLengthValue? x, TagLengthValue? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(TagLengthValue? other)
    {
        if (other is null)
            return false;

        if (other._Tag != _Tag)
            return false;

        for (nint i = 0; i < _ContentOctets.Length; i++)
        {
            if (_ContentOctets[i] != other._ContentOctets[i])
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        const int hash = 269;

        return (int) unchecked((hash * _Tag) + (hash * _ContentOctets.GetHashCode()));
    }

    public int GetHashCode(TagLengthValue obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(TagLengthValue left, TagLengthValue right) => left.Equals(right);
    public static bool operator !=(TagLengthValue left, TagLengthValue right) => !left.Equals(right);

    #endregion
}