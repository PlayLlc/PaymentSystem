using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber;
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

    public GenerateApplicationCryptogramResponse(
        IWriteIccSecuritySessionData session, CorrelationId correlation, TransactionSessionId transactionSessionId,
        GenerateApplicationCryptogramRApduSignal response) : base(correlation, MessageTypeId, transactionSessionId, response)
    {
        session.Update(this);
    }

    #endregion

    /// <exception cref="TerminalDataException"></exception>
    public static PrimitiveValue[] ResolveResponseData(IReadTlvDatabase database)
    {
        PrimitiveValue[] result = new PrimitiveValue[GetCountOfDataObjectsReturned(database)];

        int offset = 0;

        if (database.IsPresent(CryptogramInformationData.Tag))
            result[offset++] = database.Get<CryptogramInformationData>(CryptogramInformationData.Tag);
        if (database.IsPresent(ApplicationTransactionCounter.Tag))
            result[offset++] = database.Get<ApplicationTransactionCounter>(ApplicationTransactionCounter.Tag);
        if (database.IsPresent(ApplicationCryptogram.Tag))
            result[offset++] = database.Get<ApplicationCryptogram>(ApplicationCryptogram.Tag);
        if (database.IsPresent(IssuerApplicationData.Tag))
            result[offset++] = database.Get<IssuerApplicationData>(IssuerApplicationData.Tag);
        if (database.IsPresent(PosCardholderInteractionInformation.Tag))
            result[offset] = database.Get<PosCardholderInteractionInformation>(PosCardholderInteractionInformation.Tag);

        return result;
    }

    /// <exception cref="TerminalDataException"></exception>
    private static int GetCountOfDataObjectsReturned(IReadTlvDatabase database)
    {
        int offset = 0;

        if (database.IsPresent(CryptogramInformationData.Tag))
            offset++;
        if (database.IsPresent(ApplicationTransactionCounter.Tag))
            offset++;
        if (database.IsPresent(ApplicationCryptogram.Tag))
            offset++;
        if (database.IsPresent(IssuerApplicationData.Tag))
            offset++;
        if (database.IsPresent(PosCardholderInteractionInformation.Tag))
            offset++;

        return offset;
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public PrimitiveValue[] GetPrimitiveDataObjects(IWriteIccSecuritySessionData session) =>
        DecodePrimitiveValues(ResponseMessageTemplate.DecodeData(GetRApduSignal())).ToArray();

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
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