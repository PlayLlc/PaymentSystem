using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Cryptogram returned by the Card in response to the GENERATE AC or RECOVER AC command.
/// </summary>
public record ApplicationCryptogram : DataElement<ulong>, IEqualityComparer<ApplicationCryptogram>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F26;
    private const byte _ByteLength = 8;

    #endregion

    #region Constructor

    public ApplicationCryptogram(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static ApplicationCryptogram Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationCryptogram Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value) as DecodedResult<ulong>
            ?? throw new DataElementNullException(PlayEncodingId);

        return new ApplicationCryptogram(result.Value);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion

    #region Equality

    public bool Equals(ApplicationCryptogram? x, ApplicationCryptogram? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationCryptogram obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator ApplicationCryptogram(byte value) => new(value);

    #endregion
}