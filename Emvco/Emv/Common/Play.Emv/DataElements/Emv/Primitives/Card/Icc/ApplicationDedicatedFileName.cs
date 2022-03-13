using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.DataElements;

/// <summary>
///     Identifies the Dedicated File that represents an Application in a chip
/// </summary>
/// <remarks>
///     <see cref="ApplicationDedicatedFileName" /> consists of two parts. The first part is the
///     <see cref="RegisteredApplicationProviderIndicator" />
///     The second part is the Proprietary Application EncodingId Extension. The first part is required and the second part
///     is optional
/// </remarks>
public record ApplicationDedicatedFileName : DataElement<BigInteger>, IEqualityComparer<ApplicationDedicatedFileName>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;

    /// <summary>
    ///     The ApplicationDedicatedFileName requires a <see cref="RegisteredApplicationProviderIndicator" /> which is 5 bytes
    ///     in length
    /// </summary>
    /// <remarks>DecimalValue: 79; HexValue: 0x4F</remarks>
    public static readonly Tag Tag = 0x4F;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public ApplicationDedicatedFileName(BigInteger value) : base(value)
    {
        if (value.GetByteCount() < RegisteredApplicationProviderIndicator.ByteCount)
        {
            throw new DataElementParsingException(
                $"The {nameof(ApplicationDedicatedFileName)} requires a {nameof(RegisteredApplicationProviderIndicator)} but none could be found");
        }
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Value.CopyValue();
    public DedicatedFileName AsDedicatedFileName() => new DedicatedFileName(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public int GetByteCount() => _Value.Length;

    public RegisteredApplicationProviderIndicator GetRegisteredApplicationProviderIndicator() =>
        new RegisteredApplicationProviderIndicator(PlayCodec.UnsignedIntegerCodec.DecodeToUInt64(_Value[..5]));

    public override Tag GetTag() => Tag;
    public bool IsFullMatch(ApplicationDedicatedFileName other) => Equals(other);

    //public bool IsFullMatch(ApplicationIdentifier other)
    //{

    //    return Equals(other);
    //}

    //public bool IsPartialMatch(ApplicationIdentifier other)
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
    public bool IsFullMatch(DedicatedFileName other) => Equals(other);

    public bool IsPartialMatch(ApplicationDedicatedFileName other)
    {
        int comparisonLength = GetByteCount() < other.GetByteCount() ? GetByteCount() : other.GetByteCount();

        Span<byte> thisBuffer = _Value;
        Span<byte> otherBuffer = other.AsByteArray();

        for (int i = 0; i < comparisonLength; i++)
        {
            if (thisBuffer[i] != otherBuffer[i])
                return false;
        }

        return true;
    }

    public bool IsPartialMatch(DedicatedFileName other)
    {
        int comparisonLength = GetByteCount() < other.GetByteCount() ? GetByteCount() : other.GetByteCount();

        Span<byte> thisBuffer = _Value;
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

    private const byte _MinByteLength = 5;
    private const byte _MaxByteLength = 16; 

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static ApplicationDedicatedFileName Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);


    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public static ApplicationDedicatedFileName Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMinimumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value); 

        return new ApplicationDedicatedFileName(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(ApplicationDedicatedFileName? x, ApplicationDedicatedFileName? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationDedicatedFileName obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator DedicatedFileName(ApplicationDedicatedFileName value) => new DedicatedFileName(value._Value);

    #endregion
}