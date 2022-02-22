using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;

using PlayEncodingId = Play.Codecs.PlayEncodingId;

namespace Play.Icc.FileSystem.DedicatedFiles;

/// <summary>
///     The Dedicated File Name is an Application Id that is between 1 and 16 bytes long. The AID is made up of 2 data
///     elements, an internationally unique element called the Registered Id (RID 5 bytes), and an optional element called
///     the Proprietary Application ID Extension (PIX)
/// </summary>
/// <remarks>
///     The Dedicated File Name will either be identical to the Application Identifier, or it will start with the
///     Application
///     Identifier and extend it
/// </remarks>
public record DedicatedFileName : PrimitiveValue, IEqualityComparer<DedicatedFileName>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = OctetStringCodec.Identifier;

    /// <summary>
    ///     The Dedicated File Name for the File Control Information of a contact card environment. The FCI is optional in
    ///     a contact implementation
    /// </summary>
    public static DedicatedFileName PaymentSystemEnvironment = new(PlayCodec.ASCII.GetBytes("1PAY.SYS.DDF01"));

    /// <summary>
    ///     The Dedicated File Name for the File Control Information of a contactless card system. The PPSE is required
    ///     in contactless card implementations so the expectation is it will always return a response for a SELECT C-APDU
    /// </summary>
    public static DedicatedFileName ProximityPaymentSystemEnvironment = new(PlayCodec.ASCII.GetBytes("2PAY.SYS.DDF01"));

    /// <remarks>DecimalValue: 132; HexValue: 0x84</remarks>
    public static readonly Tag Tag = 0x84;

    #endregion

    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    public DedicatedFileName(ReadOnlySpan<byte> value)
    {
        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Value.CopyValue();
    public override PlayEncodingId GetEncodingId() => PlayEncodingId;
    public ushort GetByteCount() => checked((ushort) _Value.Length);

    public RegisteredApplicationProviderIndicator GetRegisteredApplicationProviderIdentifier() =>
        new(_Value.AsSpan()[..(RegisteredApplicationProviderIndicator.ByteCount - 1)]);

    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);
    public bool IsFullMatch(DedicatedFileName other) => Equals(other);

    public bool IsPartialMatch(DedicatedFileName other)
    {
        ushort comparisonLength = GetByteCount() < other.GetByteCount() ? GetByteCount() : other.GetByteCount();

        Span<byte> thisBuffer = _Value;
        Span<byte> otherBuffer = other.AsByteArray();

        for (int i = 0; i < comparisonLength; i++)
        {
            if (thisBuffer[i] != otherBuffer[i])
                return false;
        }

        return true;
    }

    public bool TryGetProprietaryApplicationIdentifier(out byte[]? pixResult)
    {
        if (_Value.Length == RegisteredApplicationProviderIndicator.ByteCount)
        {
            pixResult = null;

            return true;
        }

        pixResult = _Value[5..];

        return true;
    }

    #endregion

    #region Serialization

    public static DedicatedFileName Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static DedicatedFileName Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort minByteLength = 5;
        const ushort maxByteLength = 16;

        if (value.Length is < minByteLength and <= maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(DedicatedFileName)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        return new DedicatedFileName(value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(PlayEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(PlayEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(DedicatedFileName? x, DedicatedFileName? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DedicatedFileName obj) => obj.GetHashCode();

    #endregion
}