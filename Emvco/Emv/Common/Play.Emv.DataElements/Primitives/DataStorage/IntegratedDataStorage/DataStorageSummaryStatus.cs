using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Core.Extensions;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Information reported by the Kernel to the Terminal about:
///     • The consistency between DS Summary 1 and DS Summary 2 (successful read)
///     • The difference between DS Summary 2 and DS Summary 3 (successful write)
///     This data object is part of the Discretionary Data
/// </summary>
public record DataStorageSummaryStatus : DataElement<byte>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryCodec.Identifier;
    public static readonly Tag Tag = 0xDF810B;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public DataStorageSummaryStatus(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public bool IsReadSuccessful() => _Value.IsBitSet(Bits.Eight);
    public bool IsSuccessfulWrite() => _Value.IsBitSet(Bits.Seven);

    #endregion

    #region Serialization

    public static DataStorageSummaryStatus Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        return new DataStorageSummaryStatus(value[0]);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static DataStorageSummaryStatus Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    #endregion
}