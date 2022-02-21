using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates if the transaction performs an IDS read and/or write
/// </summary>
public record IntegratedDataStorageStatus : DataElement<byte>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryDataElementCodec.Identifier;
    public static readonly Tag Tag = 0xDF8128;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public IntegratedDataStorageStatus(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public bool IsReadSet() => _Value.IsBitSet(Bits.Eight);
    public bool IsWriteSet() => _Value.IsBitSet(Bits.Seven);
    public IntegratedDataStorageStatus SetRead() => new((byte) _Value.SetBits(Bits.Eight));
    public IntegratedDataStorageStatus SetWrite() => new((byte) _Value.SetBits(Bits.Seven));

    #endregion

    #region Serialization

    public static IntegratedDataStorageStatus Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        return new IntegratedDataStorageStatus(value[0]);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IntegratedDataStorageStatus Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    #endregion
}