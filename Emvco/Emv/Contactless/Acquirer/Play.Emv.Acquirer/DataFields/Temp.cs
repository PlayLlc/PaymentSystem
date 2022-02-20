using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record Bitmap : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 1</remarks>
    public static readonly DataFieldId DataFieldId = new(1);

    public static readonly InterchangeEncodingId EncodingId = BinaryDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public Bitmap(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override Bitmap Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Bitmap(result.Value);
    }

    #endregion
}

public record PrimaryAccountNumberPan : VariableDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 2</remarks>
    public static readonly DataFieldId DataFieldId = new(2);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 10;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public PrimaryAccountNumberPan(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override PrimaryAccountNumberPan Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new PrimaryAccountNumberPan(result.Value);
    }

    #endregion
}

public record ProcessingCode : FixedDataField<uint>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 3</remarks>
    public static readonly DataFieldId DataFieldId = new(3);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 3;

    #endregion

    #region Constructor

    public ProcessingCode(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ProcessingCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<uint> result = _Codec.Decode(EncodingId, value.Span).ToUInt32Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ProcessingCode(result.Value);
    }

    #endregion
}

public record AmountTransaction : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 4</remarks>
    public static readonly DataFieldId DataFieldId = new(4);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 6;

    #endregion

    #region Constructor

    public AmountTransaction(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override AmountTransaction Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AmountTransaction(result.Value);
    }

    #endregion
}

public record AmountSettlement : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 5</remarks>
    public static readonly DataFieldId DataFieldId = new(5);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 6;

    #endregion

    #region Constructor

    public AmountSettlement(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override AmountSettlement Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AmountSettlement(result.Value);
    }

    #endregion
}

public record AmountCardholderBilling : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 6</remarks>
    public static readonly DataFieldId DataFieldId = new(6);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 6;

    #endregion

    #region Constructor

    public AmountCardholderBilling(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override AmountCardholderBilling Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AmountCardholderBilling(result.Value);
    }

    #endregion
}

public record TransmissionDateTime : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 7</remarks>
    public static readonly DataFieldId DataFieldId = new(7);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public TransmissionDateTime(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override TransmissionDateTime Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new TransmissionDateTime(result.Value);
    }

    #endregion
}

public record AmountCardholderBillingFee : FixedDataField<uint>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 8</remarks>
    public static readonly DataFieldId DataFieldId = new(8);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 4;

    #endregion

    #region Constructor

    public AmountCardholderBillingFee(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override AmountCardholderBillingFee Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<uint> result = _Codec.Decode(EncodingId, value.Span).ToUInt32Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AmountCardholderBillingFee(result.Value);
    }

    #endregion
}

public record ConversionRateSettlement : FixedDataField<uint>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 9</remarks>
    public static readonly DataFieldId DataFieldId = new(9);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 4;

    #endregion

    #region Constructor

    public ConversionRateSettlement(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ConversionRateSettlement Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<uint> result = _Codec.Decode(EncodingId, value.Span).ToUInt32Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ConversionRateSettlement(result.Value);
    }

    #endregion
}

public record ConversionRateCardholderBilling : FixedDataField<uint>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 10</remarks>
    public static readonly DataFieldId DataFieldId = new(10);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 4;

    #endregion

    #region Constructor

    public ConversionRateCardholderBilling(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ConversionRateCardholderBilling Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<uint> result = _Codec.Decode(EncodingId, value.Span).ToUInt32Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ConversionRateCardholderBilling(result.Value);
    }

    #endregion
}

public record SystemTraceAuditNumberStan : FixedDataField<uint>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 11</remarks>
    public static readonly DataFieldId DataFieldId = new(11);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 3;

    #endregion

    #region Constructor

    public SystemTraceAuditNumberStan(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override SystemTraceAuditNumberStan Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<uint> result = _Codec.Decode(EncodingId, value.Span).ToUInt32Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new SystemTraceAuditNumberStan(result.Value);
    }

    #endregion
}

public record LocalTransactionTimeHhmmss : FixedDataField<uint>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 12</remarks>
    public static readonly DataFieldId DataFieldId = new(12);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 3;

    #endregion

    #region Constructor

    public LocalTransactionTimeHhmmss(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override LocalTransactionTimeHhmmss Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<uint> result = _Codec.Decode(EncodingId, value.Span).ToUInt32Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new LocalTransactionTimeHhmmss(result.Value);
    }

    #endregion
}

public record LocalTransactionDateMmdd : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 13</remarks>
    public static readonly DataFieldId DataFieldId = new(13);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public LocalTransactionDateMmdd(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override LocalTransactionDateMmdd Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new LocalTransactionDateMmdd(result.Value);
    }

    #endregion
}

public record ExpirationDate : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 14</remarks>
    public static readonly DataFieldId DataFieldId = new(14);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public ExpirationDate(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ExpirationDate Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ExpirationDate(result.Value);
    }

    #endregion
}

public record SettlementDate : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 15</remarks>
    public static readonly DataFieldId DataFieldId = new(15);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public SettlementDate(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override SettlementDate Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new SettlementDate(result.Value);
    }

    #endregion
}

public record CurrencyConversionDate : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 16</remarks>
    public static readonly DataFieldId DataFieldId = new(16);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public CurrencyConversionDate(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CurrencyConversionDate Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CurrencyConversionDate(result.Value);
    }

    #endregion
}

public record CaptureDate : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 17</remarks>
    public static readonly DataFieldId DataFieldId = new(17);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public CaptureDate(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CaptureDate Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CaptureDate(result.Value);
    }

    #endregion
}

public record MerchantTypeOrMerchantCategoryCode : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 18</remarks>
    public static readonly DataFieldId DataFieldId = new(18);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public MerchantTypeOrMerchantCategoryCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override MerchantTypeOrMerchantCategoryCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new MerchantTypeOrMerchantCategoryCode(result.Value);
    }

    #endregion
}

public record AcquiringInstitutionCountryCode : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 19</remarks>
    public static readonly DataFieldId DataFieldId = new(19);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public AcquiringInstitutionCountryCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override AcquiringInstitutionCountryCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AcquiringInstitutionCountryCode(result.Value);
    }

    #endregion
}

public record PanExtendedCountryCode : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 20</remarks>
    public static readonly DataFieldId DataFieldId = new(20);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public PanExtendedCountryCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override PanExtendedCountryCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new PanExtendedCountryCode(result.Value);
    }

    #endregion
}

public record ForwardingInstitutionCountryCode : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 21</remarks>
    public static readonly DataFieldId DataFieldId = new(21);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public ForwardingInstitutionCountryCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ForwardingInstitutionCountryCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ForwardingInstitutionCountryCode(result.Value);
    }

    #endregion
}

public record PointOfServiceEntryMode : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 22</remarks>
    public static readonly DataFieldId DataFieldId = new(22);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public PointOfServiceEntryMode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override PointOfServiceEntryMode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new PointOfServiceEntryMode(result.Value);
    }

    #endregion
}

public record ApplicationPanSequenceNumber : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 23</remarks>
    public static readonly DataFieldId DataFieldId = new(23);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public ApplicationPanSequenceNumber(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ApplicationPanSequenceNumber Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ApplicationPanSequenceNumber(result.Value);
    }

    #endregion
}

public record FunctionCode : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 24</remarks>
    public static readonly DataFieldId DataFieldId = new(24);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public FunctionCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override FunctionCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new FunctionCode(result.Value);
    }

    #endregion
}

public record PointOfServiceConditionCode : FixedDataField<byte>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 25</remarks>
    public static readonly DataFieldId DataFieldId = new(25);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 1;

    #endregion

    #region Constructor

    public PointOfServiceConditionCode(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override PointOfServiceConditionCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<byte> result = _Codec.Decode(EncodingId, value.Span).ToByteResult()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new PointOfServiceConditionCode(result.Value);
    }

    #endregion
}

public record PointOfServiceCaptureCode : FixedDataField<byte>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 26</remarks>
    public static readonly DataFieldId DataFieldId = new(26);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 1;

    #endregion

    #region Constructor

    public PointOfServiceCaptureCode(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override PointOfServiceCaptureCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<byte> result = _Codec.Decode(EncodingId, value.Span).ToByteResult()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new PointOfServiceCaptureCode(result.Value);
    }

    #endregion
}

public record AuthorizingIdentificationResponseLength : FixedDataField<byte>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 27</remarks>
    public static readonly DataFieldId DataFieldId = new(27);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 1;

    #endregion

    #region Constructor

    public AuthorizingIdentificationResponseLength(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override AuthorizingIdentificationResponseLength Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<byte> result = _Codec.Decode(EncodingId, value.Span).ToByteResult()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AuthorizingIdentificationResponseLength(result.Value);
    }

    #endregion
}

public record AmountTransactionFee : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 28</remarks>
    public static readonly DataFieldId DataFieldId = new(28);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public AmountTransactionFee(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override AmountTransactionFee Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AmountTransactionFee(result.Value);
    }

    #endregion
}

public record AmountSettlementFee : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 29</remarks>
    public static readonly DataFieldId DataFieldId = new(29);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public AmountSettlementFee(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override AmountSettlementFee Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AmountSettlementFee(result.Value);
    }

    #endregion
}

public record AmountTransactionProcessingFee : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 30</remarks>
    public static readonly DataFieldId DataFieldId = new(30);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public AmountTransactionProcessingFee(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override AmountTransactionProcessingFee Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AmountTransactionProcessingFee(result.Value);
    }

    #endregion
}

public record AmountSettlementProcessingFee : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 31</remarks>
    public static readonly DataFieldId DataFieldId = new(31);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public AmountSettlementProcessingFee(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override AmountSettlementProcessingFee Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AmountSettlementProcessingFee(result.Value);
    }

    #endregion
}

public record AcquiringInstitutionIdentificationCode : VariableDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 32</remarks>
    public static readonly DataFieldId DataFieldId = new(32);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 6;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public AcquiringInstitutionIdentificationCode(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override AcquiringInstitutionIdentificationCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AcquiringInstitutionIdentificationCode(result.Value);
    }

    #endregion
}

public record ForwardingInstitutionIdentificationCode : VariableDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 33</remarks>
    public static readonly DataFieldId DataFieldId = new(33);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 6;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public ForwardingInstitutionIdentificationCode(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override ForwardingInstitutionIdentificationCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ForwardingInstitutionIdentificationCode(result.Value);
    }

    #endregion
}

public record PrimaryAccountNumberExtended : VariableDataField<byte[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 34</remarks>
    public static readonly DataFieldId DataFieldId = new(34);

    public static readonly InterchangeEncodingId EncodingId = NumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 28;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public PrimaryAccountNumberExtended(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override PrimaryAccountNumberExtended Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<byte[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<byte[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new PrimaryAccountNumberExtended(result.Value);
    }

    #endregion
}

//public record Track2Data : VariableDataField<byte[]>
//{
//    /// <remarks>DecimalValue: 35</remarks>
//    public static readonly DataFieldId DataFieldId = new(35);

//    SHIIIIIIIIIIIIIIIIIIIIIIIIIT
//private const ushort _MaxByteCount = 37;
//    private const byte _LeadingOctetByteCount = 1;

//    public Track2Data(byte[] value) : base(value)
//    { }

//    public override DataFieldId GetDataFieldId() => DataFieldId;
//    public override InterchangeEncodingId GetEncodingId() => EncodingId;
//    protected override ushort GetMaxByteCount() => _MaxByteCount;
//    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

//    public override Track2Data Decode(ReadOnlyMemory<byte> value)
//    {
//        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
//        DecodedResult<byte[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<byte[]>
//        ?? throw new InterchangeDataFieldNullException(EncodingId);
//        return new Track2Data(result.Value);
//    }
//}
public record Track3Data : VariableDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 36</remarks>
    public static readonly DataFieldId DataFieldId = new(36);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 52;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public Track3Data(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Track3Data Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Track3Data(result.Value);
    }

    #endregion
}

public record RetrievalReferenceNumber : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 37</remarks>
    public static readonly DataFieldId DataFieldId = new(37);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 12;

    #endregion

    #region Constructor

    public RetrievalReferenceNumber(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override RetrievalReferenceNumber Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new RetrievalReferenceNumber(result.Value);
    }

    #endregion
}

public record AuthorizationIdentificationResponse : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 38</remarks>
    public static readonly DataFieldId DataFieldId = new(38);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 6;

    #endregion

    #region Constructor

    public AuthorizationIdentificationResponse(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override AuthorizationIdentificationResponse Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AuthorizationIdentificationResponse(result.Value);
    }

    #endregion
}

public record ResponseCode : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 39</remarks>
    public static readonly DataFieldId DataFieldId = new(39);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public ResponseCode(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ResponseCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ResponseCode(result.Value);
    }

    #endregion
}

public record ServiceRestrictionCode : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 40</remarks>
    public static readonly DataFieldId DataFieldId = new(40);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 3;

    #endregion

    #region Constructor

    public ServiceRestrictionCode(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ServiceRestrictionCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ServiceRestrictionCode(result.Value);
    }

    #endregion
}

public record CardAcceptorTerminalIdentification : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 41</remarks>
    public static readonly DataFieldId DataFieldId = new(41);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public CardAcceptorTerminalIdentification(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CardAcceptorTerminalIdentification Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CardAcceptorTerminalIdentification(result.Value);
    }

    #endregion
}

public record CardAcceptorIdentificationCode : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 42</remarks>
    public static readonly DataFieldId DataFieldId = new(42);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _ByteCount = 15;

    #endregion

    #region Constructor

    public CardAcceptorIdentificationCode(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CardAcceptorIdentificationCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CardAcceptorIdentificationCode(result.Value);
    }

    #endregion
}

public record CardAcceptorNameLocation : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 43</remarks>
    public static readonly DataFieldId DataFieldId = new(43);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _ByteCount = 40;

    #endregion

    #region Constructor

    public CardAcceptorNameLocation(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CardAcceptorNameLocation Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CardAcceptorNameLocation(result.Value);
    }

    #endregion
}

public record AdditionalResponseData : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 44</remarks>
    public static readonly DataFieldId DataFieldId = new(44);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 25;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public AdditionalResponseData(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override AdditionalResponseData Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AdditionalResponseData(result.Value);
    }

    #endregion
}

public record Track1Data : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 45</remarks>
    public static readonly DataFieldId DataFieldId = new(45);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 76;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public Track1Data(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Track1Data Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Track1Data(result.Value);
    }

    #endregion
}

public record AdditionalDataIso : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 46</remarks>
    public static readonly DataFieldId DataFieldId = new(46);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public AdditionalDataIso(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override AdditionalDataIso Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AdditionalDataIso(result.Value);
    }

    #endregion
}

public record AdditionalDataNational : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 47</remarks>
    public static readonly DataFieldId DataFieldId = new(47);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public AdditionalDataNational(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override AdditionalDataNational Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AdditionalDataNational(result.Value);
    }

    #endregion
}

public record AdditionalDataPrivate : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 48</remarks>
    public static readonly DataFieldId DataFieldId = new(48);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public AdditionalDataPrivate(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override AdditionalDataPrivate Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AdditionalDataPrivate(result.Value);
    }

    #endregion
}

public record CurrencyCodeTransaction : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 49</remarks>
    public static readonly DataFieldId DataFieldId = new(49);

    public static readonly InterchangeEncodingId EncodingId = AlphabeticDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public CurrencyCodeTransaction(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CurrencyCodeTransaction Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CurrencyCodeTransaction(result.Value);
    }

    #endregion
}

public record CurrencyCodeSettlement : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 50</remarks>
    public static readonly DataFieldId DataFieldId = new(50);

    public static readonly InterchangeEncodingId EncodingId = AlphabeticDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public CurrencyCodeSettlement(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CurrencyCodeSettlement Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CurrencyCodeSettlement(result.Value);
    }

    #endregion
}

public record CurrencyCodeCardholderBilling : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 51</remarks>
    public static readonly DataFieldId DataFieldId = new(51);

    public static readonly InterchangeEncodingId EncodingId = AlphabeticDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public CurrencyCodeCardholderBilling(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CurrencyCodeCardholderBilling Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CurrencyCodeCardholderBilling(result.Value);
    }

    #endregion
}

public record PersonalIdentificationNumberData : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 52</remarks>
    public static readonly DataFieldId DataFieldId = new(52);

    public static readonly InterchangeEncodingId EncodingId = BinaryDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public PersonalIdentificationNumberData(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override PersonalIdentificationNumberData Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new PersonalIdentificationNumberData(result.Value);
    }

    #endregion
}

public record SecurityRelatedControlInformation : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 53</remarks>
    public static readonly DataFieldId DataFieldId = new(53);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public SecurityRelatedControlInformation(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override SecurityRelatedControlInformation Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new SecurityRelatedControlInformation(result.Value);
    }

    #endregion
}

public record AdditionalAmounts : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 54</remarks>
    public static readonly DataFieldId DataFieldId = new(54);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 120;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public AdditionalAmounts(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override AdditionalAmounts Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AdditionalAmounts(result.Value);
    }

    #endregion
}

public record IccDataEmvHavingMultipleTags : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 55</remarks>
    public static readonly DataFieldId DataFieldId = new(55);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public IccDataEmvHavingMultipleTags(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override IccDataEmvHavingMultipleTags Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new IccDataEmvHavingMultipleTags(result.Value);
    }

    #endregion
}

public record Reserved56 : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 56</remarks>
    public static readonly DataFieldId DataFieldId = new(56);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public Reserved56(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Reserved56 Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Reserved56(result.Value);
    }

    #endregion
}

public record Reserved57 : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 57</remarks>
    public static readonly DataFieldId DataFieldId = new(57);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public Reserved57(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Reserved57 Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Reserved57(result.Value);
    }

    #endregion
}

public record Reserved58 : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 58</remarks>
    public static readonly DataFieldId DataFieldId = new(58);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public Reserved58(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Reserved58 Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Reserved58(result.Value);
    }

    #endregion
}

public record Reserved59 : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 59</remarks>
    public static readonly DataFieldId DataFieldId = new(59);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public Reserved59(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Reserved59 Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Reserved59(result.Value);
    }

    #endregion
}

public record Reserved60 : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 60</remarks>
    public static readonly DataFieldId DataFieldId = new(60);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public Reserved60(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Reserved60 Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Reserved60(result.Value);
    }

    #endregion
}

public record Reserved61 : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 61</remarks>
    public static readonly DataFieldId DataFieldId = new(61);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public Reserved61(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Reserved61 Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Reserved61(result.Value);
    }

    #endregion
}

public record Reserved62 : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 62</remarks>
    public static readonly DataFieldId DataFieldId = new(62);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public Reserved62(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Reserved62 Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Reserved62(result.Value);
    }

    #endregion
}

public record Reserved63 : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 63</remarks>
    public static readonly DataFieldId DataFieldId = new(63);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 999;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public Reserved63(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Reserved63 Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Reserved63(result.Value);
    }

    #endregion
}

public record MessageAuthenticationCodeMac : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 64</remarks>
    public static readonly DataFieldId DataFieldId = new(64);

    public static readonly InterchangeEncodingId EncodingId = BinaryDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public MessageAuthenticationCodeMac(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override MessageAuthenticationCodeMac Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new MessageAuthenticationCodeMac(result.Value);
    }

    #endregion
}

public record ExtendedBitmapIndicator : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 65</remarks>
    public static readonly DataFieldId DataFieldId = new(65);

    public static readonly InterchangeEncodingId EncodingId = BinaryDataFieldCodec.Identifier;
    private const ushort _ByteCount = 1;

    #endregion

    #region Constructor

    public ExtendedBitmapIndicator(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ExtendedBitmapIndicator Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ExtendedBitmapIndicator(result.Value);
    }

    #endregion
}

public record SettlementCode : FixedDataField<byte>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 66</remarks>
    public static readonly DataFieldId DataFieldId = new(66);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 1;

    #endregion

    #region Constructor

    public SettlementCode(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override SettlementCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<byte> result = _Codec.Decode(EncodingId, value.Span).ToByteResult()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new SettlementCode(result.Value);
    }

    #endregion
}

public record ExtendedPaymentCode : FixedDataField<byte>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 67</remarks>
    public static readonly DataFieldId DataFieldId = new(67);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 1;

    #endregion

    #region Constructor

    public ExtendedPaymentCode(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ExtendedPaymentCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<byte> result = _Codec.Decode(EncodingId, value.Span).ToByteResult()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ExtendedPaymentCode(result.Value);
    }

    #endregion
}

public record ReceivingInstitutionCountryCode : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 68</remarks>
    public static readonly DataFieldId DataFieldId = new(68);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public ReceivingInstitutionCountryCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ReceivingInstitutionCountryCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ReceivingInstitutionCountryCode(result.Value);
    }

    #endregion
}

public record SettlementInstitutionCountryCode : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 69</remarks>
    public static readonly DataFieldId DataFieldId = new(69);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public SettlementInstitutionCountryCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override SettlementInstitutionCountryCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new SettlementInstitutionCountryCode(result.Value);
    }

    #endregion
}

public record NetworkManagementInformationCode : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 70</remarks>
    public static readonly DataFieldId DataFieldId = new(70);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public NetworkManagementInformationCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override NetworkManagementInformationCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new NetworkManagementInformationCode(result.Value);
    }

    #endregion
}

public record MessageNumber : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 71</remarks>
    public static readonly DataFieldId DataFieldId = new(71);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public MessageNumber(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override MessageNumber Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new MessageNumber(result.Value);
    }

    #endregion
}

public record LastMessagesNumber : FixedDataField<ushort>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 72</remarks>
    public static readonly DataFieldId DataFieldId = new(72);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public LastMessagesNumber(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override LastMessagesNumber Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value.Span).ToUInt16Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new LastMessagesNumber(result.Value);
    }

    #endregion
}

public record ActionDateYymmdd : FixedDataField<uint>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 73</remarks>
    public static readonly DataFieldId DataFieldId = new(73);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 3;

    #endregion

    #region Constructor

    public ActionDateYymmdd(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ActionDateYymmdd Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<uint> result = _Codec.Decode(EncodingId, value.Span).ToUInt32Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ActionDateYymmdd(result.Value);
    }

    #endregion
}

public record NumberOfCredits : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 74</remarks>
    public static readonly DataFieldId DataFieldId = new(74);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public NumberOfCredits(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override NumberOfCredits Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new NumberOfCredits(result.Value);
    }

    #endregion
}

public record CreditsReversalNumber : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 75</remarks>
    public static readonly DataFieldId DataFieldId = new(75);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public CreditsReversalNumber(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CreditsReversalNumber Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CreditsReversalNumber(result.Value);
    }

    #endregion
}

public record NumberOfDebits : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 76</remarks>
    public static readonly DataFieldId DataFieldId = new(76);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public NumberOfDebits(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override NumberOfDebits Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new NumberOfDebits(result.Value);
    }

    #endregion
}

public record DebitsReversalNumber : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 77</remarks>
    public static readonly DataFieldId DataFieldId = new(77);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public DebitsReversalNumber(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override DebitsReversalNumber Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new DebitsReversalNumber(result.Value);
    }

    #endregion
}

public record TransferNumber : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 78</remarks>
    public static readonly DataFieldId DataFieldId = new(78);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public TransferNumber(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override TransferNumber Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new TransferNumber(result.Value);
    }

    #endregion
}

public record TransferReversalNumber : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 79</remarks>
    public static readonly DataFieldId DataFieldId = new(79);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public TransferReversalNumber(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override TransferReversalNumber Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new TransferReversalNumber(result.Value);
    }

    #endregion
}

public record NumberOfInquiries : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 80</remarks>
    public static readonly DataFieldId DataFieldId = new(80);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public NumberOfInquiries(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override NumberOfInquiries Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new NumberOfInquiries(result.Value);
    }

    #endregion
}

public record NumberOfAuthorizations : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 81</remarks>
    public static readonly DataFieldId DataFieldId = new(81);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public NumberOfAuthorizations(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override NumberOfAuthorizations Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new NumberOfAuthorizations(result.Value);
    }

    #endregion
}

public record CreditsProcessingFeeAmount : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 82</remarks>
    public static readonly DataFieldId DataFieldId = new(82);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 6;

    #endregion

    #region Constructor

    public CreditsProcessingFeeAmount(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CreditsProcessingFeeAmount Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CreditsProcessingFeeAmount(result.Value);
    }

    #endregion
}

public record CreditsTransactionFeeAmount : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 83</remarks>
    public static readonly DataFieldId DataFieldId = new(83);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 6;

    #endregion

    #region Constructor

    public CreditsTransactionFeeAmount(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CreditsTransactionFeeAmount Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CreditsTransactionFeeAmount(result.Value);
    }

    #endregion
}

public record DebitsProcessingFeeAmount : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 84</remarks>
    public static readonly DataFieldId DataFieldId = new(84);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 6;

    #endregion

    #region Constructor

    public DebitsProcessingFeeAmount(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override DebitsProcessingFeeAmount Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new DebitsProcessingFeeAmount(result.Value);
    }

    #endregion
}

public record DebitsTransactionFeeAmount : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 85</remarks>
    public static readonly DataFieldId DataFieldId = new(85);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 6;

    #endregion

    #region Constructor

    public DebitsTransactionFeeAmount(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override DebitsTransactionFeeAmount Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new DebitsTransactionFeeAmount(result.Value);
    }

    #endregion
}

public record TotalAmountOfCredits : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 86</remarks>
    public static readonly DataFieldId DataFieldId = new(86);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public TotalAmountOfCredits(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override TotalAmountOfCredits Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new TotalAmountOfCredits(result.Value);
    }

    #endregion
}

public record CreditsReversalAmount : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 87</remarks>
    public static readonly DataFieldId DataFieldId = new(87);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public CreditsReversalAmount(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override CreditsReversalAmount Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CreditsReversalAmount(result.Value);
    }

    #endregion
}

public record TotalAmountOfDebits : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 88</remarks>
    public static readonly DataFieldId DataFieldId = new(88);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public TotalAmountOfDebits(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override TotalAmountOfDebits Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new TotalAmountOfDebits(result.Value);
    }

    #endregion
}

public record DebitsReversalAmount : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 89</remarks>
    public static readonly DataFieldId DataFieldId = new(89);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public DebitsReversalAmount(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override DebitsReversalAmount Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new DebitsReversalAmount(result.Value);
    }

    #endregion
}

public record OriginalDataElements : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 90</remarks>
    public static readonly DataFieldId DataFieldId = new(90);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 21;

    #endregion

    #region Constructor

    public OriginalDataElements(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override OriginalDataElements Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new OriginalDataElements(result.Value);
    }

    #endregion
}

public record FileUpdateCode : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 91</remarks>
    public static readonly DataFieldId DataFieldId = new(91);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 1;

    #endregion

    #region Constructor

    public FileUpdateCode(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override FileUpdateCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new FileUpdateCode(result.Value);
    }

    #endregion
}

public record FileSecurityCode : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 92</remarks>
    public static readonly DataFieldId DataFieldId = new(92);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public FileSecurityCode(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override FileSecurityCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new FileSecurityCode(result.Value);
    }

    #endregion
}

public record ResponseIndicator : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 93</remarks>
    public static readonly DataFieldId DataFieldId = new(93);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public ResponseIndicator(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ResponseIndicator Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ResponseIndicator(result.Value);
    }

    #endregion
}

public record ServiceIndicator : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 94</remarks>
    public static readonly DataFieldId DataFieldId = new(94);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 7;

    #endregion

    #region Constructor

    public ServiceIndicator(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ServiceIndicator Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ServiceIndicator(result.Value);
    }

    #endregion
}

public record ReplacementAmounts : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 95</remarks>
    public static readonly DataFieldId DataFieldId = new(95);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 42;

    #endregion

    #region Constructor

    public ReplacementAmounts(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override ReplacementAmounts Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ReplacementAmounts(result.Value);
    }

    #endregion
}

public record MessageSecurityCode : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 96</remarks>
    public static readonly DataFieldId DataFieldId = new(96);

    public static readonly InterchangeEncodingId EncodingId = BinaryDataFieldCodec.Identifier;
    private const ushort _ByteCount = 64;

    #endregion

    #region Constructor

    public MessageSecurityCode(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override MessageSecurityCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new MessageSecurityCode(result.Value);
    }

    #endregion
}

public record NetSettlementAmount : FixedDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 97</remarks>
    public static readonly DataFieldId DataFieldId = new(97);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 16;

    #endregion

    #region Constructor

    public NetSettlementAmount(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override NetSettlementAmount Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new NetSettlementAmount(result.Value);
    }

    #endregion
}

public record Payee : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 98</remarks>
    public static readonly DataFieldId DataFieldId = new(98);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _ByteCount = 25;

    #endregion

    #region Constructor

    public Payee(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override Payee Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new Payee(result.Value);
    }

    #endregion
}

public record SettlementInstitutionIdentificationCode : VariableDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 99</remarks>
    public static readonly DataFieldId DataFieldId = new(99);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 6;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public SettlementInstitutionIdentificationCode(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override SettlementInstitutionIdentificationCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new SettlementInstitutionIdentificationCode(result.Value);
    }

    #endregion
}

public record ReceivingInstitutionIdentificationCode : VariableDataField<BigInteger>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 100</remarks>
    public static readonly DataFieldId DataFieldId = new(100);

    public static readonly InterchangeEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 6;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public ReceivingInstitutionIdentificationCode(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override ReceivingInstitutionIdentificationCode Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value.Span).ToBigInteger()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new ReceivingInstitutionIdentificationCode(result.Value);
    }

    #endregion
}

public record FileName : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 101</remarks>
    public static readonly DataFieldId DataFieldId = new(101);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 17;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public FileName(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override FileName Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new FileName(result.Value);
    }

    #endregion
}

public record AccountIdentification1 : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 102</remarks>
    public static readonly DataFieldId DataFieldId = new(102);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 28;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public AccountIdentification1(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override AccountIdentification1 Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AccountIdentification1(result.Value);
    }

    #endregion
}

public record AccountIdentification2 : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 103</remarks>
    public static readonly DataFieldId DataFieldId = new(103);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 28;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public AccountIdentification2(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override AccountIdentification2 Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new AccountIdentification2(result.Value);
    }

    #endregion
}

public record TransactionDescription : VariableDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 104</remarks>
    public static readonly DataFieldId DataFieldId = new(104);

    public static readonly InterchangeEncodingId EncodingId = AlphaNumericSpecialDataFieldCodec.Identifier;
    private const ushort _MaxByteCount = 100;
    private const byte _LeadingOctetByteCount = 2;

    #endregion

    #region Constructor

    public TransactionDescription(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override TransactionDescription Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForMaximumLength(value, _MaxByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new TransactionDescription(result.Value);
    }

    #endregion
}