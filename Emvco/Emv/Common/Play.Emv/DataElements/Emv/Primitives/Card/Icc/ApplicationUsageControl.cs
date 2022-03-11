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

    public static ApplicationUsageControl Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static ApplicationUsageControl Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value) as DecodedResult<ushort>
            ?? throw new DataElementParsingException(
                $"The {nameof(ApplicationUsageControl)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        return new ApplicationUsageControl(result.Value);
    }

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