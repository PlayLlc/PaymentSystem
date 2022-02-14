﻿using System.Collections.Immutable;

using Play.Codecs;
using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.Codecs;

internal class InterchangeCodec
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<DataFieldIdTypes, (DataFieldMetadata, PlayEncoding)> _CodecMap;

    #endregion

    #region Constructor

    static InterchangeCodec()
    {
        _CodecMap = new Dictionary<DataFieldIdTypes, (DataFieldMetadata, PlayEncoding)>()
        {
            {DataFieldIdTypes.PrimaryAccountNumber, (new DataFieldMetadata(null, 19), PlayEncoding.Numeric)}

            // .. etc
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public void GetBytes(List<byte> buffer, int offset, DataField dataField)
    {
        (DataFieldMetadata, PlayEncoding) codec = _CodecMap[dataField.GetDataFieldId()];

        // Logic for max & min length validation?

        dataField.CopyTo(buffer);
    }

    #endregion
}