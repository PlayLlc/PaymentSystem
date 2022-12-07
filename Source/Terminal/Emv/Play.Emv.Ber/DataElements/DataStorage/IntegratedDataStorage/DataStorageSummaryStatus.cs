using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Information reported by the Kernel to the Terminal about:
///     • The consistency between DS Summary 1 and DS Summary 2 (successful read)
///     • The difference between DS Summary 2 and DS Summary 3 (successful write)
///     This data object is part of the Discretionary Data
/// </summary>
public record DataStorageSummaryStatus : DataElement<byte>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF810B;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public DataStorageSummaryStatus(byte value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageSummaryStatus Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DataStorageSummaryStatus Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageSummaryStatus Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new DataStorageSummaryStatus(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public bool IsReadSuccessful() => _Value.IsBitSet(Bits.Eight);
    public bool IsSuccessfulWrite() => _Value.IsBitSet(Bits.Seven);
    public static Builder GetBuilder() => new();

    #endregion

    public class Builder : PrimitiveValueBuilder<byte>
    {
        #region Constructor

        internal Builder(DataStorageSummaryStatus outcomeParameterSet)
        {
            _Value = outcomeParameterSet._Value;
        }

        internal Builder()
        { }

        #endregion

        #region Instance Members

        public void Reset(DataStorageSummaryStatus value)
        {
            _Value = value._Value;
        }

        public override DataStorageSummaryStatus Complete() => new(_Value);
        public bool IsReadSuccessful() => _Value.IsBitSet(Bits.Eight);
        public bool IsSuccessfulWrite() => _Value.IsBitSet(Bits.Seven);

        public void SetReadIsSuccessful(bool value)
        {
            if (value)
                _Value.SetBit(Bits.Eight);

            _Value.ClearBits(Bits.Eight);
        }

        public void SetWriteIsSuccessful(bool value)
        {
            if (value)
                _Value.SetBit(Bits.Seven);

            _Value.ClearBits(Bits.Seven);
        }

        protected override void Set(byte bitsToSet)
        {
            _Value |= bitsToSet;
        }

        #endregion
    }
}