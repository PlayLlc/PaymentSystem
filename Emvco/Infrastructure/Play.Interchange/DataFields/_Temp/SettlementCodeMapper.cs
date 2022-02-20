﻿using Play.Codecs;
using Play.Codecs.Strings;

namespace Play.Interchange.Messages.DataFields._Temp;

public class SettlementCodeMapper : FixedLengthDataFieldMapper
{
    #region Static Metadata

    public static readonly DataFieldId DataFieldId = new(66);
    public static readonly PlayEncodingId PlayEncodingId = Numeric.PlayEncodingId;
    private const byte _ByteLength = 1;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    #endregion
}