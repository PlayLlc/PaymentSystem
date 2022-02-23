﻿using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class PayeeCodec : FixedLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 98</remarks>
    public static readonly DataFieldId DataFieldId = new(98);

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    private const ushort _ByteLength = 25;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}