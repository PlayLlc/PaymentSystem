using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.DataFields;

namespace Play.Emv.Interchange.DataFields;

/// <summary>
///     The encrypted PIN Block
/// </summary>
public record EncipheredPinData : InterchangeDataField
{
    #region Static Metadata

    public static readonly DataFieldId DataFieldId = new(44);
    public static readonly PlayEncodingId PlayEncodingId = AlphaNumeric.PlayEncodingId;
    private const ushort _MaxByteLength = 25;
    private const byte _LeadingOctetLength = 1;

    #endregion
}