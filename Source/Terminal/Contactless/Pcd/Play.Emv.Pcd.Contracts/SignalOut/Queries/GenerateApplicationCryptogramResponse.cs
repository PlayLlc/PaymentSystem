using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
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
        CorrelationId correlation, TransactionSessionId transactionSessionId, GenerateApplicationCryptogramRApduSignal response) : base(correlation,
        MessageTypeId, transactionSessionId, response)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    ///     This method resolves data objects in the same order that were received by a previous
    ///     <see cref="GenerateApplicationCryptogramResponse" />
    /// </summary>
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

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public PrimitiveValue[] GetPrimitiveDataObjects(IReadTlvDatabase database)
    {
        if (GetDataByteCount() == 0)
            return Array.Empty<PrimitiveValue>();

        if (_Codec.DecodeFirstTag(GetData()) == ResponseMessageTemplateFormat2.Tag)
            ValidateFormat2Response(database);

        PrimitiveValue[] results = DecodePrimitiveValues(ResponseMessageTemplate.DecodeData(GetRApduSignal())).ToArray();
        ValidateResponseTemplate(results);

        return results;
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

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private static IEnumerable<PrimitiveValue> DecodePrimitiveValues(TagLengthValue[] values)
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

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private static void ValidateFormat2Response(IReadTlvDatabase database)
    {
        if (database.IsPresentAndNotEmpty(CryptogramInformationData.Tag))
        {
            throw new DataElementParsingException(
                $"Parsing has failed because the required object: [{nameof(CryptogramInformationData)}] returned from the {nameof(GenerateApplicationCryptogramResponse)} is already present and not empty in the database");
        }

        if (database.IsPresentAndNotEmpty(ApplicationTransactionCounter.Tag))
        {
            throw new DataElementParsingException(
                $"Parsing has failed because the required object: [{nameof(ApplicationTransactionCounter)}] returned from the {nameof(GenerateApplicationCryptogramResponse)} is already present and not empty in the database");
        }

        if (database.IsPresentAndNotEmpty(ApplicationCryptogram.Tag))
        {
            throw new DataElementParsingException(
                $"Parsing has failed because the required object: [{nameof(ApplicationCryptogram)}] returned from the {nameof(GenerateApplicationCryptogramResponse)} is already present and not empty in the database");
        }
    }

    /// <exception cref="DataElementParsingException"></exception>
    private static void ValidateResponseTemplate(PrimitiveValue[] values)
    {
        if (values.All(a => a.GetTag() != CryptogramInformationData.Tag))
        {
            throw new DataElementParsingException(
                $"Parsing has failed because the required object: [{nameof(CryptogramInformationData)}] could not be retrieved from the {nameof(GenerateApplicationCryptogramResponse)}");
        }

        if (values.All(a => a.GetTag() != ApplicationTransactionCounter.Tag))
        {
            throw new DataElementParsingException(
                $"Parsing has failed because the required object: [{nameof(ApplicationTransactionCounter)}] could not be retrieved from the {nameof(GenerateApplicationCryptogramResponse)}");
        }

        if (values.All(a => a.GetTag() != ApplicationCryptogram.Tag))
        {
            throw new DataElementParsingException(
                $"Parsing has failed because the required object: [{nameof(ApplicationCryptogram)}] could not be retrieved from the {nameof(GenerateApplicationCryptogramResponse)}");
        }
    }

    #endregion
}