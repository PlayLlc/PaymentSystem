using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record GenerateApplicationCryptogramResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GenerateApplicationCryptogramResponse));

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="correlation"></param>
    /// <param name="transactionSessionId"></param>
    /// <param name="response"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public GenerateApplicationCryptogramResponse(
        CorrelationId correlation, TransactionSessionId transactionSessionId, GenerateApplicationCryptogramRApduSignal response) :
        base(correlation, MessageTypeId, transactionSessionId, response)
    { }

    #endregion

    /// <exception cref="BerParsingException"></exception>
    public PrimitiveValue[] GetPrimitiveDataObjects() =>
        DecodePrimitiveValues(ResponseMessageTemplate.DecodeData(GetRApduSignal())).ToArray();

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private IEnumerable<PrimitiveValue> DecodePrimitiveValues(TagLengthValue[] values)
    {
        // TODO: Validate mandatory data objects
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i].GetTag() == CryptogramInformationData.Tag)
                yield return CryptogramInformationData.Decode(values[i].EncodeValue().AsSpan());
            else if (values[i].GetTag() == ApplicationTransactionCounter.Tag)
                yield return ApplicationTransactionCounter.Decode(values[i].EncodeValue().AsSpan());
            else if (values[i].GetTag() == ApplicationCryptogram.Tag)
                yield return ApplicationCryptogram.Decode(values[i].EncodeValue().AsSpan());
            else if (values[i].GetTag() == IssuerApplicationData.Tag)
                yield return IssuerApplicationData.Decode(values[i].EncodeValue().AsSpan());
            else if (values[i].GetTag() == PosCardholderInteractionInformation.Tag)
                yield return PosCardholderInteractionInformation.Decode(values[i].EncodeValue().AsSpan());
        }
    }
}