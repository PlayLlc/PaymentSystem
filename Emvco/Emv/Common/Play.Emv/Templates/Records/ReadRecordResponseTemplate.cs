﻿using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Icc.Messaging.Apdu.ReadRecord;

namespace Play.Emv.Templates;

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
    public static TagLengthValue[] GetRecords(ReadOnlySpan<byte> value) => _Codec.DecodeTagLengthValues(value);

    #endregion
}