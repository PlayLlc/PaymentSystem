using Play.Emv.Acquirers.Elavon.DataElements;

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
        foreach (ElavonDataElement<dynamic>? element in elavonDataElements)
            element.EncodeTagLengthValue(buffer, ref offset);
    }

    #endregion
}