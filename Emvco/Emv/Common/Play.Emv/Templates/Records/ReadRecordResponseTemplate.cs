using System;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.Templates.Records;

public abstract class ReadRecordResponseTemplate : Template
{
    #region Static Metadata

    public static readonly Tag Tag = 0x70;

    #endregion

    #region Instance Values

    protected byte[] _Values;

    #endregion

    #region Constructor

    protected ReadRecordResponseTemplate()
    {
        _Values = Array.Empty<byte>();
    }

    protected ReadRecordResponseTemplate(byte[] values)
    {
        _Values = values;
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Values;
    public TagLengthValue[] GetPrimitiveValues(BerCodec berCodec) => berCodec.DecodeTagLengthValues(_Values.AsSpan());

    #endregion
}