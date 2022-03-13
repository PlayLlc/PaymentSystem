using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the capabilities of the card to support specific functions in the application
/// </summary>
public record ApplicationInterchangeProfile : DataElement<ushort>, IEqualityComparer<ApplicationInterchangeProfile>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x82;

    #endregion

    #region Constructor

    public ApplicationInterchangeProfile(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
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
      

    private const byte _ByteLength = 2;




    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static ApplicationInterchangeProfile Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);


    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static ApplicationInterchangeProfile Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);
         

        return new ApplicationInterchangeProfile(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

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