using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Core.Exceptions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Issuer.Messages;

namespace Play.Emv.Issuer.Exceptions;

internal class Check
{
    #region Static Metadata

    public static readonly CheckCore Core = new();

    #endregion

    public class Request
    {
        #region Instance Members

        public static void ForRequiredDataElement(
            Dictionary<Tag, TagLengthValue> values,
            Tag requiredDataElement,
            AcquirerMessageId acquirerMessageId)
        {
            if (!values.ContainsKey(requiredDataElement))
            {
                throw new AcquirerMessageMissingRequiredData(
                    $"The {nameof(AcquirerMessage)} with the {nameof(AcquirerMessageId)}: [{acquirerMessageId}] could not be initialized. The {nameof(AcquirerMessage)} requires the Data Element with the {nameof(Tag)}: [{requiredDataElement}], but the object was not provided");
            }
        }

        public static void ForConditionalDataElement(
            Dictionary<Tag, TagLengthValue> values,
            Tag conditional,
            AcquirerMessageId acquirerMessageId)
        {
            if (!values.ContainsKey(conditional))
            {
                throw new AcquirerMessageMissingRequiredData(
                    $"The {nameof(AcquirerMessage)} with the {nameof(AcquirerMessageId)}: [{acquirerMessageId}] could not be initialized. The {nameof(AcquirerMessage)} requires the Data Element with the {nameof(Tag)}: [{conditional}] to ensure it has all of the conditional values needed by the Acquirer but the object was not provided");
            }
        }

        #endregion
    }
}