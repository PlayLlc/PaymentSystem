using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the issuer's specified restrictions on the geographic use and services allowed for the application.
/// </summary>
public record ApplicationUsageControl : DataElement<ushort>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F07;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public ApplicationUsageControl(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization
     
     




    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static ApplicationUsageControl Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);


    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static ApplicationUsageControl Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);
         

        return new ApplicationUsageControl(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Byte 1

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsValidForDomesticCashTransactions() => ((byte) (_Value >> 8)).IsBitSet(Bits.Eight);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsValidForInternationalCashTransactions() => ((byte) (_Value >> 8)).IsBitSet(Bits.Seven);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsValidForDomesticGoods() => ((byte) (_Value >> 8)).IsBitSet(Bits.Six);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsValidForInternationalGoods() => ((byte) (_Value >> 8)).IsBitSet(Bits.Five);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsValidForInternationalServices() => ((byte) (_Value >> 8)).IsBitSet(Bits.Three);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsValidAtAtms() => ((byte) (_Value >> 8)).IsBitSet(Bits.Two);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsValidAtTerminalsOtherThanAtms() => ((byte) (_Value >> 8)).IsBitSet(Bits.One);

    #endregion

    #region Byte 2

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsDomesticCashbackAllowed() => ((byte) _Value).IsBitSet(Bits.Eight);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsInternationalCashbackAllowed() => ((byte) _Value).IsBitSet(Bits.Seven);

    #endregion
}