﻿using Play.Codecs;
using Play.Codecs.Strings;

namespace Play.Interchange.Messages.DataFields._Temp;

public class ExtendedBitmapIndicatorMapper : FixedLengthDataFieldMapper
{
    #region Static Metadata

    public static readonly DataFieldId DataFieldId = new(65);
    public static readonly PlayEncodingId PlayEncodingId = Binary.PlayEncodingId;
    private const byte _ByteLength = 1;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    #endregion
}