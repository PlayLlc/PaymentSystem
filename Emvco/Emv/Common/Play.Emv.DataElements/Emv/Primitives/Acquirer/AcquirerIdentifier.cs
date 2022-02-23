using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Uniquely identifies the acquirer within each payment system
/// </summary>
public record AcquirerIdentifier : DataElement<ulong>, IEqualityComparer<AcquirerIdentifier>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F01;
    private const byte _ByteLength = 6;
    private const byte _MinCharLength = 6;
    private const byte _MaxCharLength = 11;

    #endregion

    #region Constructor

    public AcquirerIdentifier(ulong value) : base(value)
    {
        Validate(value);
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    public void Validate(ulong value)
    {
        if (value.GetMostSignificantByte() > _ByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(AcquirerIdentifier)} could not be initialized because the byte length provided was out of range. The byte length was {value.GetMostSignificantByte()} but must be {_ByteLength} bytes in length");
        }

        if (value.GetNumberOfDigits() is < _MinCharLength and <= _MaxCharLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(AcquirerIdentifier)} could not be initialized because the decoded character length was out of range. The decoded character length was {value.GetNumberOfDigits()} but must be in the range of {_MinCharLength}-{_MaxCharLength}");
        }
    }

    #endregion

    #region Serialization

    public static AcquirerIdentifier Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static AcquirerIdentifier Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value).ToUInt64Result()
            ?? throw new DataElementNullException(PlayEncodingId);

        return new AcquirerIdentifier(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(PlayEncodingId, _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(AcquirerIdentifier? x, AcquirerIdentifier? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(AcquirerIdentifier obj) => obj.GetHashCode();

    #endregion
}