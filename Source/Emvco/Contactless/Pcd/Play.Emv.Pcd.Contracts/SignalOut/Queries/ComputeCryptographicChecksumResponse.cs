using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ComputeCryptographicChecksumResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ReadRecordResponse));

    #endregion

    #region Constructor

    public ComputeCryptographicChecksumResponse(CorrelationId correlationId, TransactionSessionId transactionSessionId, ReadRecordRApduSignal rApdu) : base(
        correlationId, MessageTypeId, transactionSessionId, rApdu)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="BerParsingException"></exception>
    public PrimitiveValue[] GetPrimitiveDataObjects() =>

        // TODO: Handle for mandatory objects not present. Set Level 2 error
        DecodePrimitiveValues(ResponseMessageTemplate.DecodeData(GetRApduSignal())).ToArray();

    /// <summary>
    ///     DecodePrimitiveValues
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Play.Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private IEnumerable<PrimitiveValue> DecodePrimitiveValues(TagLengthValue[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i].GetTag() == ApplicationTransactionCounter.Tag)
                yield return ApplicationTransactionCounter.Decode(values[i].EncodeValue().AsSpan());
            else if (values[i].GetTag() == CardholderVerificationCode3Track1.Tag)
                yield return CardholderVerificationCode3Track1.Decode(values[i].EncodeValue().AsSpan());
            else if (values[i].GetTag() == CardholderVerificationCode3Track2.Tag)
                yield return CardholderVerificationCode3Track2.Decode(values[i].EncodeValue().AsSpan());
            else if (values[i].GetTag() == PosCardholderInteractionInformation.Tag)
                yield return PosCardholderInteractionInformation.Decode(values[i].EncodeValue().AsSpan());
        }
    }

    public int GetValueByteCount() => GetData().Length;

    #endregion
}