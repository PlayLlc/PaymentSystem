﻿using System;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record GenerateApplicationCryptogramResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GenerateApplicationCryptogramResponse));

    #endregion

    #region Instance Values

    private readonly CryptogramInformationData _CryptogramInformationData;
    private readonly ApplicationTransactionCounter _ApplicationTransactionCounter;
    private readonly ApplicationCryptogram _ApplicationCryptogram;
    private readonly IssuerApplicationData? _IssuerApplicationData;

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
        CorrelationId correlation,
        TransactionSessionId transactionSessionId,
        GenerateApplicationCryptogramRApduSignal response) : base(correlation, MessageTypeId, transactionSessionId, response)
    {
        //HACK: We should be parsing this on demand rather than immediately after we transceive
        GenerateApplicationCryptogramResponseMetadata a = DecodeData(response);
        _CryptogramInformationData = a.CryptogramInformationData;
        _ApplicationTransactionCounter = a.ApplicationTransactionCounter;
        _ApplicationCryptogram = a.ApplicationCryptogram;
        _IssuerApplicationData = a.IssuerApplicationData;
    }

    #endregion

    #region Instance Members

    public CryptogramInformationData GetCryptogramInformationData() => _CryptogramInformationData;
    public ApplicationTransactionCounter GetApplicationTransactionCounter() => _ApplicationTransactionCounter;
    public ApplicationCryptogram GetApplicationCryptogram() => _ApplicationCryptogram;

    public bool TryGetIssuerApplicationData(out IssuerApplicationData? result)
    {
        if (_IssuerApplicationData is null)
        {
            result = null;

            return false;
        }

        result = _IssuerApplicationData;

        return true;
    }

    /// <summary>
    ///     DecodeData
    /// </summary>
    /// <param name="rapdu"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private static GenerateApplicationCryptogramResponseMetadata DecodeData(GenerateApplicationCryptogramRApduSignal rapdu)
    {
        TagLengthValue[] a = ResponseMessageTemplate.DecodeData(rapdu);

        CryptogramInformationData cryptogramInformationData =
            CryptogramInformationData.Decode(a.First(a => a.GetTag() == CryptogramInformationData.Tag).GetValue().AsSpan());
        ApplicationTransactionCounter applicationTransactionCounter =
            ApplicationTransactionCounter.Decode(a.First(a => a.GetTag() == ApplicationTransactionCounter.Tag).GetValue().AsSpan());
        ApplicationCryptogram applicationCryptogram =
            ApplicationCryptogram.Decode(a.First(a => a.GetTag() == ApplicationCryptogram.Tag).GetValue().AsSpan());

        if (!a.Any(a => a.GetTag() == IssuerApplicationData.Tag))
        {
            return new GenerateApplicationCryptogramResponseMetadata(cryptogramInformationData, applicationTransactionCounter,
                                                                     applicationCryptogram, null);
        }

        IssuerApplicationData issuerApplicationData =
            IssuerApplicationData.Decode(a.First(a => a.GetTag() == IssuerApplicationData.Tag).GetValue().AsSpan());

        return new GenerateApplicationCryptogramResponseMetadata(cryptogramInformationData, applicationTransactionCounter,
                                                                 applicationCryptogram, issuerApplicationData);
    }

    #endregion

    private record GenerateApplicationCryptogramResponseMetadata(
        CryptogramInformationData CryptogramInformationData,
        ApplicationTransactionCounter ApplicationTransactionCounter,
        ApplicationCryptogram ApplicationCryptogram,
        IssuerApplicationData? IssuerApplicationData);
}