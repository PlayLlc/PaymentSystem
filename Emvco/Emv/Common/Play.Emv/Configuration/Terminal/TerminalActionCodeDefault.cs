using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.DataElements.Emv;

namespace Play.Emv.Configuration;

/// <summary>
///     Description: Specifies the acquirer's conditions that cause a transaction to be rejected on an offline only
///     Terminal.
/// </summary>
public record TerminalActionCodeDefault : PrimitiveValue, IEqualityComparer<TerminalActionCodeDefault>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly TerminalActionCodeDefault Default = new(0x840000000C);
    public static readonly Tag Tag = 0xDF8120;

    #endregion

    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public TerminalActionCodeDefault(ulong value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public ActionCodes AsActionCodes() => new(_Value);
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static TerminalActionCodeDefault Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TerminalActionCodeDefault Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort byteLength = 5;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TerminalActionCodeDefault)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ulong> result = codec.Decode(BerEncodingId, value) as DecodedResult<ulong>
            ?? throw new InvalidOperationException(
                $"The {nameof(TerminalActionCodeDefault)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        return new TerminalActionCodeDefault(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(TerminalActionCodeDefault? x, TerminalActionCodeDefault? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TerminalActionCodeDefault obj) => obj.GetHashCode();

    #endregion
}