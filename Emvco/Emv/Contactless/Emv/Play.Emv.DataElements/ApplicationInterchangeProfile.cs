﻿using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Core.Extensions;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the capabilities of the card to support specific functions in the application
/// </summary>
public record ApplicationInterchangeProfile : DataElement<ushort>, IEqualityComparer<ApplicationInterchangeProfile>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x82;

    #endregion

    #region Constructor

    public ApplicationInterchangeProfile(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);
    public bool IsCardholderVerificationSupported() => _Value.IsBitSet(13);

    /// <summary>
    ///     Combined Dynamic Data Authentication and Application Cryptogram Generation
    /// </summary>
    public bool IsCombinedDataAuthenticationSupported() => _Value.IsBitSet(9);

    public bool IsDynamicDataAuthenticationSupported() => _Value.IsBitSet(14);
    public bool IsIssuerAuthenticationSupported() => _Value.IsBitSet(11);
    public bool IsStaticDataAuthenticationSupported() => _Value.IsBitSet(15);
    public bool IsTerminalRiskManagementRequired() => _Value.IsBitSet(12);
    public byte[] Encode() => new[] {(byte) (_Value >> 8), (byte) _Value};

    #endregion

    #region Serialization

    public static ApplicationInterchangeProfile Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationInterchangeProfile Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 2;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ApplicationInterchangeProfile)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ushort> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<ushort>
            ?? throw new InvalidOperationException(
                $"The {nameof(ApplicationInterchangeProfile)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        return new ApplicationInterchangeProfile(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(ApplicationInterchangeProfile? x, ApplicationInterchangeProfile? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationInterchangeProfile obj) => obj.GetHashCode();

    #endregion
}