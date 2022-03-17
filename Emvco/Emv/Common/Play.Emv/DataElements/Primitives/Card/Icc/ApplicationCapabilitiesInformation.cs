using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Description: Lists a number of card features beyond regular payment.
/// </summary>
public record ApplicationCapabilitiesInformation : DataElement<uint>, IEqualityComparer<ApplicationCapabilitiesInformation>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F5D;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    public ApplicationCapabilitiesInformation(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool CombinedDataAuthenticationIndicator() => _Value.IsBitSet(9);
    public override PlayEncodingId GetEncodingId() => EncodingId;

    public SdsSchemeIndicator GetSdsSchemeIndicator()
    {
        const byte bitOffset = 1;

        return SdsSchemeIndicator.Get((byte) (_Value >> bitOffset));
    }

    public DataStorageVersionNumber GetDataStorageVersionNumber()
    {
        const byte bitOffset = 16;
        const byte bitMask = 0b00111111;

        return new DataStorageVersionNumber((byte) (_Value >> bitOffset).GetMaskedValue(bitMask));
    }

    public override Tag GetTag() => Tag;
    public bool SupportForBalanceReading() => _Value.IsBitSet(10);
    public bool IsSupportForFieldOffDetectionSet() => _Value.IsBitSet(11);
    public byte[] Encode() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);

    #endregion

    #region Serialization

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static ApplicationCapabilitiesInformation Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static ApplicationCapabilitiesInformation Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.NumericCodec.DecodeToUInt16(value);

        return new ApplicationCapabilitiesInformation(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(ApplicationCapabilitiesInformation? x, ApplicationCapabilitiesInformation? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationCapabilitiesInformation obj) => obj.GetHashCode();

    #endregion
}