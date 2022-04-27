using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Icc.Exceptions;

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
public record DedicatedFileName : PrimitiveValue
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = HexadecimalCodec.EncodingId;

    /// <summary>
    ///     The Dedicated File Name for the File Control Information of a contact card environment. The FCI is optional in
    ///     a contact implementation
    /// </summary>
    public static DedicatedFileName PaymentSystemEnvironment = new(PlayCodec.AsciiCodec.Encode("1PAY.SYS.DDF01"));

    /// <summary>
    ///     The Dedicated File Name for the File Control Information of a contactless card system. The PPSE is required
    ///     in contactless card implementations so the expectation is it will always return a response for a SELECT C-APDU
    /// </summary>
    public static DedicatedFileName ProximityPaymentSystemEnvironment = new(PlayCodec.AsciiCodec.Encode("2PAY.SYS.DDF01"));

    /// <remarks>DecimalValue: 132; HexValue: 0x84</remarks>
    public static readonly Tag Tag = 0x84;

    private const byte _MinByteLength = 5;
    private const byte _MaxByteLength = 16;

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
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public ushort GetByteCount() => checked((ushort) _Value.Length);

    public RegisteredApplicationProviderIndicator GetRegisteredApplicationProviderIdentifier() =>
        new(_Value.AsSpan()[..(RegisteredApplicationProviderIndicator.ByteCount - 1)]);

    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
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
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    public static DedicatedFileName Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        CheckCore.ForMaximumLength(value, _MaxByteLength, nameof(DedicatedFileName));
        CheckCore.ForMinimumLength(value, _MinByteLength, nameof(DedicatedFileName));

        return new DedicatedFileName(value);
    }

    public override DedicatedFileName Decode(TagLengthValue value)
    {
        Span<byte> buffer = value.EncodeValue();
        CheckCore.ForMaximumLength(buffer, _MaxByteLength, nameof(DedicatedFileName));
        CheckCore.ForMinimumLength(buffer, _MinByteLength, nameof(DedicatedFileName));

        return new DedicatedFileName(buffer);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

    #endregion
}