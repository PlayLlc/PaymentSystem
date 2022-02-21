﻿using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.DataFields._Temp;

internal class CurrencyCodeSettlementCodec : FixedLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 50</remarks>
    public static readonly DataFieldId DataFieldId = new(50);

    public static readonly PlayEncodingId PlayEncodingId = Alphabetic.PlayEncodingId;
    private const ushort _ByteLength = 2;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    #endregion
}