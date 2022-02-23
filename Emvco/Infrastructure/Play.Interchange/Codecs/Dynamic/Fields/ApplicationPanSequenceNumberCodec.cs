﻿using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class ApplicationPanSequenceNumberCodec : FixedLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 23</remarks>
    public static readonly DataFieldId DataFieldId = new(23);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteLength = 2;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}