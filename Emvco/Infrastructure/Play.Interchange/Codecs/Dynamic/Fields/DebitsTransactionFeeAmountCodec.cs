﻿using Play.Codecs;
using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class DebitsTransactionFeeAmountCodec : FixedLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 85</remarks>
    public static readonly DataFieldId DataFieldId = new(85);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteLength = 6;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}