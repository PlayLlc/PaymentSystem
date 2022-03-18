using System;
using System.Collections.Generic;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Time-variant number generated by the ICC, to be captured by the terminal
/// </summary>
public record IccDynamicNumber : DataElement<ulong>, IEqualityComparer<IccDynamicNumber>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F4C;
    private const byte _MinByteLength = 2;
    private const byte _MaxByteLength = 8;

    #endregion

    #region Constructor

    public IccDynamicNumber(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static IccDynamicNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static IccDynamicNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        ulong result = PlayCodec.BinaryCodec.DecodeToUInt64(value);

        return new IccDynamicNumber(result);
    }

    #endregion

    #region Equality

    public bool Equals(IccDynamicNumber? x, IccDynamicNumber? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IccDynamicNumber obj) => obj.GetHashCode();

    #endregion
}