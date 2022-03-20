﻿using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Security.Authentications.Offline.CombinedDataAuthentication;
using Play.Emv.Security.Certificates.Icc;

namespace Play.Emv.Security.Messages.CDA;

/// <summary>
///     Command for authenticating the dynamic signature for the first GENERATE AC command using the CDA flag
/// </summary>
public class AuthenticateCombinedData1Command
{
    #region Instance Values

    private readonly DataObjectListResult _CardRiskManagementDataObjectList1Result;
    private readonly GenerateAcCdaResponseMessage _GenerateAcCdaResponseMessage;
    private readonly DecodedIccPublicKeyCertificate _IccPublicKeyCertificate;
    private readonly DataObjectListResult _ProcessingOptionsDataObjectListResult;
    private readonly UnpredictableNumber _UnpredictableNumber;

    #endregion

    #region Constructor

    public AuthenticateCombinedData1Command(
        DecodedIccPublicKeyCertificate iccPublicKeyCertificate,
        UnpredictableNumber unpredictableNumber,
        DataObjectListResult processingOptionsDataObjectListResult,
        DataObjectListResult cardRiskManagementDataObjectList1Result,
        GenerateAcCdaResponseMessage generateAcCdaResponseMessage,
        SignedDynamicApplicationData signedDynamicApplicationData)
    {
        _IccPublicKeyCertificate = iccPublicKeyCertificate;
        _UnpredictableNumber = unpredictableNumber;
        _ProcessingOptionsDataObjectListResult = processingOptionsDataObjectListResult;
        _CardRiskManagementDataObjectList1Result = cardRiskManagementDataObjectList1Result;
        _GenerateAcCdaResponseMessage = generateAcCdaResponseMessage;
    }

    #endregion

    #region Instance Members

    public GenerateAcCdaResponseMessage GetGenerateAcCdaResponseMessage() => _GenerateAcCdaResponseMessage;
    public DecodedIccPublicKeyCertificate GetIccPublicKeyCertificate() => _IccPublicKeyCertificate;

    public byte[] GetTransactionDataHashCodeInput(BerCodec codec)
    {
        List<byte> buffer = new();

        PrimitiveValue[]? pdolResult = _ProcessingOptionsDataObjectListResult.AsPrimitiveValues();
        PrimitiveValue[]? cdolResult = _CardRiskManagementDataObjectList1Result.AsPrimitiveValues();

        for (int i = 0; i < pdolResult.Length; i++)
            buffer.AddRange(pdolResult[i].EncodeValue(EmvCodec.GetBerCodec()));

        for (int i = 0; i < cdolResult.Length; i++)
            buffer.AddRange(cdolResult[i].EncodeValue(EmvCodec.GetBerCodec()));

        buffer.AddRange(_GenerateAcCdaResponseMessage.GetTransactionDataHashData(codec));

        return buffer.ToArray();
    }

    public UnpredictableNumber GetUnpredictableNumber() => _UnpredictableNumber;

    #endregion
}