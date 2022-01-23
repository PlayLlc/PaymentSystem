using System;

using Play.Ber.Identifiers;
using Play.Ber.Lengths;

namespace Play.Ber.InternalFactories;

/// <summary>
///     The metadata fields of an encoded BER TLV object
/// </summary>
public readonly record struct TagLength
{
    #region Instance Values

    private readonly Length _Length;
    private readonly Tag _Tag;

    #endregion

    #region Constructor

    public TagLength(Tag tag, ReadOnlySpan<byte> contentOctets)
    {
        _Tag = tag;
        Length length = new(contentOctets);
        _Length = new Length(contentOctets);
    }

    public TagLength(Tag tag, Length length)
    {
        _Tag = tag;
        _Length = length;
    }

    public TagLength(Tag tag, byte length)
    {
        _Tag = tag;
        _Length = new Length(length);
    }

    public TagLength(Tag tag, ushort length)
    {
        _Tag = tag;
        _Length = new Length(length);
    }

    public TagLength(Tag tag, uint length)
    {
        _Tag = tag;
        _Length = new Length(length);
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Encode
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    public byte[] Encode()
    {
        int byteLength = GetTagLengthByteCount();
        Span<byte> result = stackalloc byte[byteLength];

        GetTag().Serialize().CopyTo(result);

        Length length = GetLength();
        byte[]? lengthBuffer = GetLength().Serialize();

        GetLength().Serialize().CopyTo(result[GetTag().GetByteCount()..]);

        return result.ToArray();
    }

    public Length GetLength() => _Length;

    /// <summary>
    ///     The byte count of the Length field this object represents
    /// </summary>
    public byte GetLengthByteCount() => _Length.GetByteCount();

    public Tag GetTag() => _Tag;

    /// <summary>
    ///     The byte count of the Tag field this object represents
    /// </summary>
    public byte GetTagByteCount() => _Tag.GetByteCount();

    /// <summary>
    ///     The byte count of the Tag and Length fields this object represents
    /// </summary>
    /// <returns></returns>
    public int GetTagLengthByteCount() => GetTagByteCount() + GetLengthByteCount();

    /// <summary>
    ///     The byte count of the Tag, Length and Value fields this object represents
    /// </summary>
    public int GetTagLengthValueByteCount() => GetTagByteCount() + GetLengthByteCount() + GetValueByteCount();

    /// <summary>
    ///     The byte count of the Value field this object represents
    /// </summary>
    public ushort GetValueByteCount() => _Length.GetContentLength();

    /// <summary>
    ///     The byte count of the Tag and Length used to get the start index
    ///     of the Value content in a sequence of bytes
    /// </summary>
    /// <returns></returns>
    internal byte GetValueOffset() => (byte) (_Tag.GetByteCount() + _Length.GetByteCount());

    /// <summary>
    ///     The zero based range for the Value field. The offset is defaulted at zero
    /// </summary>
    internal Range ValueRange() => new(GetValueOffset(), GetValueByteCount() + GetValueOffset());

    #endregion
}