using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Time.Seconds;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: This data object is used to indicate to the Terminal the outcome of the transaction processing by the
///     Kernel. Its value is an accumulation of results about applicable parts of the transaction.
/// </summary>
public record OutcomeParameterSet : DataElement<ulong>, IEqualityComparer<OutcomeParameterSet>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly OutcomeParameterSet Default;
    public static readonly Tag Tag = 0xDF8129;
    private const byte _StatusOutcomeOffset = 56;
    private const byte _StartOutcomeOffset = 48;
    private const byte _OnlineResponseOutcomeOffset = 40;
    private const byte _CvmOutcomeOffset = 32;
    private const byte _AlternateInterfaceOutcomeOffset = 16;
    private const byte _FieldOffRequestOutcomeOffset = 8;
    private const byte _ByteLength = 8;

    #endregion

    #region Constructor

    static OutcomeParameterSet()
    {
        Builder builder = GetBuilder();
        builder.Set(StartOutcome.NotAvailable);
        builder.Set(CvmPerformedOutcome.NotAvailable);
        builder.Set(OnlineResponseOutcome.NotAvailable);
        Default = builder.Complete();
    }

    private OutcomeParameterSet(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public AlternateInterfacePreferenceOutcome GetAlternateInterfacePreferenceOutcome() =>
        AlternateInterfacePreferenceOutcome.Get((byte) (_Value >> _AlternateInterfaceOutcomeOffset));

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static Builder GetBuilder() => new();
    public CvmPerformedOutcome GetCvmPerformed() => CvmPerformedOutcome.Get((byte) (_Value >> _CvmOutcomeOffset));
    public FieldOffRequestOutcome GetFieldOffRequestOutcome() => new((byte) (_Value >> _FieldOffRequestOutcomeOffset));
    public OnlineResponseOutcome GetOnlineResponseOutcome() => OnlineResponseOutcome.Get((byte) (_Value >> _OnlineResponseOutcomeOffset));
    public StartOutcome GetStartOutcome() => StartOutcome.Get((byte) (_Value >> _StartOutcomeOffset));
    public StatusOutcome GetStatusOutcome() => StatusOutcome.Get((byte) (_Value >> _StatusOutcomeOffset));
    public override Tag GetTag() => Tag;

    public Milliseconds GetTimeout()
    {
        const byte bitOffset = (8 - 1) * 8;

        return new Milliseconds((byte) (_Value >> bitOffset));
    }

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public bool IsDataRecordPresent() => _Value.IsBitSet(38);
    public bool IsDiscretionaryDataPresent() => _Value.IsBitSet(37);
    public bool IsReceiptPresent() => _Value.IsBitSet(36);
    public bool IsTimeout() => GetOnlineResponseOutcome() == OnlineResponseOutcome.NotAvailable;
    public bool IsUiRequestOnOutcomePresent() => _Value.IsBitSet(40);
    public bool IsUiRequestOnRestartPresent() => _Value.IsBitSet(39);

    #endregion

    #region Serialization

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static OutcomeParameterSet Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override OutcomeParameterSet Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static OutcomeParameterSet Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.NumericCodec.DecodeToUInt64(value);

        return new OutcomeParameterSet(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(OutcomeParameterSet? x, OutcomeParameterSet? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(OutcomeParameterSet obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static OutcomeParameterSet operator |(OutcomeParameterSet left, OutcomeParameterSet right) => new(left._Value | right._Value);

    #endregion

    public class Builder : PrimitiveValueBuilder<ulong>
    {
        #region Constructor

        internal Builder(OutcomeParameterSet outcomeParameterSet)
        {
            _Value = outcomeParameterSet._Value;
        }

        internal Builder()
        {
            // Initialize all values as their default values
            Set(StatusOutcome.NotAvailable);
            Set(StartOutcome.NotAvailable);
            Set(OnlineResponseOutcome.NotAvailable);
            Set(CvmPerformedOutcome.NotAvailable);
            Set(AlternateInterfacePreferenceOutcome.NotAvailable);
            Set(FieldOffRequestOutcome.NotAvailable);
        }

        #endregion

        #region Instance Members

        public void Reset(OutcomeParameterSet value)
        {
            _Value = value._Value;
        }

        public void Set(StatusOutcome bitsToSet)
        {
            _Value.ClearBits((ulong) byte.MaxValue << _StatusOutcomeOffset);
            _Value |= (ulong) bitsToSet << _StatusOutcomeOffset;
        }

        public void Set(StartOutcome bitsToSet)
        {
            _Value.ClearBits((ulong) byte.MaxValue << _StartOutcomeOffset);
            _Value |= (ulong) bitsToSet << _StartOutcomeOffset;
        }

        public void Set(OnlineResponseOutcome bitsToSet)
        {
            _Value.ClearBits((ulong) byte.MaxValue << _OnlineResponseOutcomeOffset);
            _Value |= (ulong) bitsToSet << _OnlineResponseOutcomeOffset;
        }

        public void Set(CvmPerformedOutcome bitsToSet)
        {
            _Value.ClearBits((ulong) byte.MaxValue << _CvmOutcomeOffset);
            _Value |= (ulong) bitsToSet << _CvmOutcomeOffset;
        }

        public void Set(AlternateInterfacePreferenceOutcome bitsToSet)
        {
            _Value.ClearBits((ulong) byte.MaxValue << _AlternateInterfaceOutcomeOffset);
            _Value |= (ulong) bitsToSet << _AlternateInterfaceOutcomeOffset;
        }

        public void Set(FieldOffRequestOutcome bitsToSet)
        {
            _Value.ClearBits((ulong) byte.MaxValue << _FieldOffRequestOutcomeOffset);
            _Value |= (ulong) bitsToSet << _FieldOffRequestOutcomeOffset;
        }

        public void Set(Milliseconds bitsToSet)
        {
            const byte bitOffset = (8 - 1) * 8;
            _Value.ClearBits((ulong) byte.MaxValue << bitOffset);
            _Value |= (ulong) ((long) bitsToSet << bitOffset);
        }

        // TODO: Rename this to Complete, this is confusing
        public override OutcomeParameterSet Complete() => new(_Value);

        public void SetIsDataRecordPresent(bool value)
        {
            if (value)
                _Value.SetBit(38);

            _Value.ClearBit(38);
        }

        public void SetIsDiscretionaryDataPresent(bool value)
        {
            if (value)
                _Value.SetBit(37);

            _Value.ClearBit(37);
        }

        public void SetIsReceiptPresent(bool value)
        {
            if (value)
                _Value.SetBit(36);

            _Value.ClearBit(36);
        }

        public void SetIsUiRequestOnOutcomePresent(bool value)
        {
            if (value)
                _Value.SetBit(40);

            _Value.ClearBit(40);
        }

        public void SetIsUiRequestOnRestartPresent(bool value)
        {
            if (value)
                _Value.SetBit(39);

            _Value.ClearBit(39);
        }

        protected override void Set(ulong bitsToSet)
        {
            _Value |= bitsToSet;
        }

        #endregion
    }
}