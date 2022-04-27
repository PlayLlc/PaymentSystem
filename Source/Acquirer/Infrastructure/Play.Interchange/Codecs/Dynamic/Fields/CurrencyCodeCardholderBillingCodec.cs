﻿using Play.Codecs;
using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class CurrencyCodeCardholderBillingCodec : FixedLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 51</remarks>
    public static readonly DataFieldId DataFieldId = new(51);

    public static readonly PlayEncodingId EncodingId = AlphabeticCodec.EncodingId;
    private const ushort _ByteLength = 2;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}