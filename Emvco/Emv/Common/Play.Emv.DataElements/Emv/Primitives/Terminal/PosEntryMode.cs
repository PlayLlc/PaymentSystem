using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

public record PosEntryMode : DataElement<byte>, IEqualityComparer<PosEntryMode>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F39;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public PosEntryMode(byte value) : base(value)
    { }

    public PosEntryMode(PosEntryModeTypes value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static PosEntryMode Decode(ReadOnlyMemory<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = _Codec.Decode(PlayEncodingId, value.Span) as DecodedResult<byte>
            ?? throw new DataElementNullException(PlayEncodingId);

        return new PosEntryMode(result.Value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static PosEntryMode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = _Codec.Decode(EncodingId, value).ToByteResult() ?? throw new DataElementNullException(PlayEncodingId);

        return new PosEntryMode(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(PlayEncodingId, _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(PosEntryMode? x, PosEntryMode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(PosEntryMode obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator byte(PosEntryMode value) => value._Value;

    #endregion
}