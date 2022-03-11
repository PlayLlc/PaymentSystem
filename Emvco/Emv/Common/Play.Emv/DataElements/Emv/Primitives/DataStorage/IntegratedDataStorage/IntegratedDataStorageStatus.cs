using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates if the transaction performs an IDS read and/or write
/// </summary>
public record IntegratedDataStorageStatus : DataElement<byte>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8128;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public IntegratedDataStorageStatus(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public bool IsReadSet() => _Value.IsBitSet(Bits.Eight);
    public bool IsWriteSet() => _Value.IsBitSet(Bits.Seven);

    public IntegratedDataStorageStatus SetRead(bool value) =>
        value ? new IntegratedDataStorageStatus(_Value.SetBits(Bits.Eight)) : new IntegratedDataStorageStatus(_Value.ClearBits(Bits.Eight));

    public IntegratedDataStorageStatus SetWrite(bool value) =>
        value ? new IntegratedDataStorageStatus(_Value.SetBits(Bits.Seven)) : new IntegratedDataStorageStatus(_Value.ClearBits(Bits.Seven));

    #endregion

    #region Serialization

    public static IntegratedDataStorageStatus Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        return new IntegratedDataStorageStatus(value[0]);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IntegratedDataStorageStatus Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    #endregion
}