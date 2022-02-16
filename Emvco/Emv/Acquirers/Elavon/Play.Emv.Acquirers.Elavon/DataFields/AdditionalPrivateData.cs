using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.DataObjects;
using Play.Core.Specifications;
using Play.Emv.Acquirers.Elavon.DataElements;
using Play.Interchange.Messages.DataFields;

namespace Play.Emv.Acquirers.Elavon.DataFields;

/// <summary>
///     This will be used to send requests and responses related to services not dealt with by other fields. This data
///     element utilizes the Tag, Length, Value (TLV) structure to delineate different sub-elements in the following
///     manner:
///     [LLL][TAG][LENGTH][VALUE][TAG][LENGTH][VALUE] ………
/// </summary>
internal class AdditionalPrivateData
{
    #region Instance Members

    public static void Encode(Span<byte> buffer, ref int offset, params ElavonDataElement<dynamic>[] elavonDataElements)
    {
        foreach (var element in elavonDataElements)
            element.EncodeTagLengthValue(buffer, ref offset);
    }

    #endregion
}