﻿using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class CreditsReversalAmountCodec : FixedLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 87</remarks>
    public static readonly DataFieldId DataFieldId = new(87);

    public static readonly PlayEncodingId PlayEncodingId = Numeric.PlayEncodingId;
    private const ushort _ByteLength = 8;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    #endregion
}