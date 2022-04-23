using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the functions performed in a transaction
/// </summary>
public record TransactionStatusInformation : DataElement<ushort>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9B;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    private TransactionStatusInformation() : base(0)
    { }

    private TransactionStatusInformation(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static TransactionStatusInformation Create() => new();
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    public TransactionStatusInformation Set(TransactionStatusInformationFlags transactionStatus)
    {
        if (transactionStatus == TransactionStatusInformationFlags.NotAvailable)
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

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TransactionStatusInformation Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TransactionStatusInformation Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TransactionStatusInformation Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new TransactionStatusInformation(result);
    }

    public new byte[] EncodeValue() => Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion
}