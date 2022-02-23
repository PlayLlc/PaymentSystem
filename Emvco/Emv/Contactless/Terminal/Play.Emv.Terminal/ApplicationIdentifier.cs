using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Icc.FileSystem.DedicatedFiles;


namespace Play.Emv.Terminal;

/// <summary>
///     Identifies the application as described in ISO/IEC 7816-5
/// </summary>
public record ApplicationIdentifier : PrimitiveValue, IEqualityComparer<ApplicationIdentifier>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F06;

    #endregion

    #region Instance Values

    private readonly BigInteger _Value;

    #endregion

    #region Constructor

    public ApplicationIdentifier(BigInteger value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Value.ToByteArray();
    public override PlayEncodingId GetEncodingId() => EncodingId;

    public RegisteredApplicationProviderIndicator GetRegisteredApplicationProviderIndicator() =>
        new(PlayCodec.UnsignedIntegerCodec.DecodeToUInt64(_Value.ToByteArray()[..5]));

    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public int GetValueByteCount() => _Value.GetByteCount();

    //public bool IsFullMatch(ApplicationDedicatedFileName other)
    //{
    //    return Equals(other);
    //}

    //public bool IsPartialMatch(ApplicationDedicatedFileName other)
    //{
    //    int comparisonLength = GetTagLengthValueByteCount() < other.GetTagLengthValueByteCount()
    //        ? GetTagLengthValueByteCount()
    //        : other.GetTagLengthValueByteCount();

    //    Span<byte> thisBuffer = _Value.ToByteArray();
    //    Span<byte> otherBuffer = other.AsByteArray();

    //    for (int i = 0; i < comparisonLength; i++)
    //    {
    //        if (thisBuffer[i] != otherBuffer[i])
    //            return false;
    //    }

    //    return true;
    //}

    //public bool IsFullMatch(ApplicationIdentifier other)
    //{

    //    return Equals(other);
    //}

    public bool IsPartialMatch(ApplicationIdentifier other)
    {
        int comparisonLength = GetValueByteCount() < other.GetValueByteCount() ? GetValueByteCount() : other.GetValueByteCount();

        Span<byte> thisBuffer = _Value.ToByteArray();
        Span<byte> otherBuffer = other.AsByteArray();

        for (int i = 0; i < comparisonLength; i++)
        {
            if (thisBuffer[i] != otherBuffer[i])
                return false;
        }

        return true;
    }

    #endregion

    #region Serialization

    public static ApplicationIdentifier Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationIdentifier Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort minByteLength = 5;
        const ushort maxByteLength = 16;

        if (value.Length is not >= minByteLength and <= maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ApplicationIdentifier)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        DecodedResult<BigInteger> result = codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(ApplicationIdentifier)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new ApplicationIdentifier(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(ApplicationIdentifier? x, ApplicationIdentifier? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationIdentifier obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(ApplicationIdentifier left, byte right) => left._Value == right;
    public static bool operator ==(byte left, ApplicationIdentifier right) => left == right._Value;
    public static bool operator !=(ApplicationIdentifier left, byte right) => !(left == right);
    public static bool operator !=(byte left, ApplicationIdentifier right) => !(left == right);

    #endregion

    //public bool IsFullMatch(DedicatedFileName other)
    //{

    //    return Equals(other);
    //}

    //public bool IsPartialMatch(DedicatedFileName other)
    //{
    //    int comparisonLength = GetTagLengthValueByteCount() < other.GetTagLengthValueByteCount()
    //        ? GetTagLengthValueByteCount()
    //        : other.GetTagLengthValueByteCount();

    //    Span<byte> thisBuffer = _Value.ToByteArray();
    //    Span<byte> otherBuffer = other.AsByteArray();

    //    for (int i = 0; i < comparisonLength; i++)
    //    {
    //        if (thisBuffer[i] != otherBuffer[i])
    //            return false;
    //    }

    //    return true;
    //}
}