﻿using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Icc.Exceptions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record RecoverAcResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(RecoverAcResponse));

    #endregion

    #region Constructor

    public RecoverAcResponse(
        CorrelationId correlationId, TransactionSessionId transactionSessionId, RecoverApplicationCryptogramRApduSignal rApdu) : base(
        correlationId, MessageTypeId, transactionSessionId, rApdu)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public PrimitiveValue[] GetPrimitiveDataObjects()
    {
        PrimitiveValue[] result = DecodePrimitiveValues(ResponseMessageTemplate.DecodeData(GetRApduSignal())).ToArray();
        ValidatePrimitiveDataObjects(result);

        return result;
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
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
            else if (values[i].GetTag() == SignedDynamicApplicationData.Tag)
                yield return SignedDynamicApplicationData.Decode(values[i].EncodeValue().AsSpan());
            else if (values[i].GetTag() == ApplicationCryptogram.Tag)
                yield return ApplicationCryptogram.Decode(values[i].EncodeValue().AsSpan());
            else if (values[i].GetTag() == IssuerApplicationData.Tag)
                yield return IssuerApplicationData.Decode(values[i].EncodeValue().AsSpan());
            else if (values[i].GetTag() == PosCardholderInteractionInformation.Tag)
                yield return PosCardholderInteractionInformation.Decode(values[i].EncodeValue().AsSpan());
        }
    }

    /// <exception cref="IccProtocolException"></exception>
    private void ValidatePrimitiveDataObjects(PrimitiveValue[] values)
    {
        CryptogramInformationData? cid =
            (CryptogramInformationData) values.FirstOrDefault(a => a.GetTag() == CryptogramInformationData.Tag)!;

        if (cid is null)
        {
            throw new IccProtocolException(
                $"The required object: [{nameof(CryptogramInformationData)}] was missing from the {nameof(RecoverAcResponse)}");
        }

        if (cid!.IsCdaSignatureRequested())
            ValidateCdaPerformed(values);
        else
            ValidateCdaNotPerformed(values);
    }

    /// <exception cref="IccProtocolException"></exception>
    private void ValidateCdaPerformed(PrimitiveValue[] values)
    {
        if (values.All(a => a.GetTag() != ApplicationTransactionCounter.Tag))
        {
            throw new IccProtocolException(
                $"The object: [{nameof(ApplicationTransactionCounter)}] was required when CDA is not requested but was missing from the {nameof(RecoverAcResponse)}");
        }

        if (values.All(a => a.GetTag() != SignedDynamicApplicationData.Tag))
        {
            throw new IccProtocolException(
                $"The object: [{nameof(SignedDynamicApplicationData)}] was required when CDA is not requested but was missing from the {nameof(RecoverAcResponse)}");
        }
    }

    /// <exception cref="IccProtocolException"></exception>
    private void ValidateCdaNotPerformed(PrimitiveValue[] values)
    {
        if (values.All(a => a.GetTag() != ApplicationTransactionCounter.Tag))
        {
            throw new IccProtocolException(
                $"The object: [{nameof(ApplicationTransactionCounter)}] was required when CDA is not requested but was missing from the {nameof(RecoverAcResponse)}");
        }

        if (values.All(a => a.GetTag() != ApplicationCryptogram.Tag))
        {
            throw new IccProtocolException(
                $"The object: [{nameof(ApplicationCryptogram)}] was required when CDA is not requested but was missing from the {nameof(RecoverAcResponse)}");
        }
    }

    #endregion
}