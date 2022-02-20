﻿using Play.Codecs;
using Play.Codecs.Strings;

namespace Play.Interchange.Messages.DataFields._Temp;

public class AdditionalDataNationalMapper : VariableLengthDataFieldMapper
{
    #region Static Metadata

    public static readonly DataFieldId DataFieldId = new(47);
    public static readonly PlayEncodingId PlayEncodingId = AlphaNumeric.PlayEncodingId;
    private const ushort _MaxByteLength = 999;
    private const byte _LeadingOctetLength = 2;

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;
    protected override ushort GetMaxByteLength() => _MaxByteLength;
    protected override ushort GetLeadingOctetLength() => _LeadingOctetLength;

    #endregion
}