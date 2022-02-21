﻿using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class FileNameCodec : VariableLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 101</remarks>
    public static readonly DataFieldId DataFieldId = new(101);

    public static readonly PlayEncodingId PlayEncodingId = AlphaNumericSpecial.PlayEncodingId;
    private const ushort _MaxByteLength = 17;
    private const byte _LeadingOctetLength = 1;

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;
    protected override ushort GetMaxByteLength() => _MaxByteLength;
    protected override ushort GetLeadingOctetLength() => _LeadingOctetLength;

    #endregion
}