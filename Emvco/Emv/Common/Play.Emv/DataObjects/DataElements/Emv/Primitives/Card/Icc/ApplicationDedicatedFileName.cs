using System;
using System.Collections.Generic;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Identifies the Dedicated File that represents an Application in a chip
/// </summary>
/// <remarks>
///     <see cref="ApplicationDedicatedFileName" /> consists of two parts. The first part is the
///     <see cref="RegisteredApplicationProviderIndicator" />
///     The second part is the Proprietary Application EncodingId Extension. The first part is required and the second part
///     is optional
/// </remarks>
public record ApplicationDedicatedFileName : DataElement<byte[]>, IEqualityComparer<ApplicationDedicatedFileName>
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

    public ApplicationDedicatedFileName(ReadOnlySpan<byte> value) : base(value.ToArray())
    {
        if (_Value.Length < RegisteredApplicationProviderIndicator.ByteCount)
        {
            throw new ArgumentOutOfRangeException(
                $"The {nameof(ApplicationDedicatedFileName)} requires a {nameof(RegisteredApplicationProviderIndicator)} but none could be found");
        }
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Value.CopyValue();
    public DedicatedFileName AsDedicatedFileName() => new(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public int GetByteCount() => _Value.Length;

    public RegisteredApplicationProviderIndicator GetRegisteredApplicationProviderIndicator() =>
        new(PlayCodec.UnsignedIntegerCodec.DecodeToUInt64(_Value[..5]));

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

    public static ApplicationDedicatedFileName Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationDedicatedFileName Decode(ReadOnlySpan<byte> value)
    {
        const ushort minByteLength = 5;
        const ushort maxByteLength = 16;

        if (value.Length is < minByteLength and <= maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ApplicationDedicatedFileName)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        return new ApplicationDedicatedFileName(value);
    }

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

    public static implicit operator DedicatedFileName(ApplicationDedicatedFileName value) => new(value._Value);

    #endregion
}