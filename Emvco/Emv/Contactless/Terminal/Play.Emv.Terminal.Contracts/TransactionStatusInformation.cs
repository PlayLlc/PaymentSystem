using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Core.Extensions;

using BinaryCodec = Play.Emv.Ber.Codecs.BinaryCodec;

namespace Play.Emv.Terminal.Contracts;

/// <summary>
///     Indicates the functions performed in a transaction
/// </summary>
public record TransactionStatusInformation : PrimitiveValue, IEqualityComparer<TransactionStatusInformation>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9B;

    #endregion

    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    private TransactionStatusInformation()
    { }

    private TransactionStatusInformation(ushort value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static TransactionStatusInformation Create() => new();
    public override PlayEncodingId GetEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    public TransactionStatusInformation Set(TransactionStatusInformationFlagTypes transactionStatus)
    {
        if (transactionStatus == TransactionStatusInformationFlagTypes.NotAvailable)
            return this;

        return new TransactionStatusInformation((ushort) (_Value | transactionStatus));
    }

    public bool CardholderVerificationWasPerformed() => _Value.IsBitSet(7);
    public bool CardRiskManagementWasPerformed() => _Value.IsBitSet(6);
    public bool IssuerAuthenticationWasPerformed() => _Value.IsBitSet(5);
    public bool OfflineDataAuthenticationWasPerformed() => _Value.IsBitSet(8);
    public bool ScriptProcessingWasPerformed() => _Value.IsBitSet(3);
    public bool TerminalRiskManagementWasPerformed() => _Value.IsBitSet(4);

    #endregion

    #region Serialization

    public static TransactionStatusInformation Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TransactionStatusInformation Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort byteLength = 2;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TransactionStatusInformation)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ushort> result = codec.Decode(PlayEncodingId, value) as DecodedResult<ushort>
            ?? throw new InvalidOperationException(
                $"The {nameof(TransactionStatusInformation)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        return new TransactionStatusInformation(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(PlayEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(PlayEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(TransactionStatusInformation? x, TransactionStatusInformation? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TransactionStatusInformation obj) => obj.GetHashCode();

    #endregion
}