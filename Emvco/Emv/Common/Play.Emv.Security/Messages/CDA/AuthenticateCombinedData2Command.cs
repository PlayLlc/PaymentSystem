using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Emv.Primitives.Security;
using Play.Emv.Security.Authentications.Offline.CombinedDataAuthentication;
using Play.Emv.Security.Certificates.Icc;
using Play.Emv.Security.Cryptograms;

namespace Play.Emv.Security.Messages.CDA;

/// <summary>
///     Command for authenticating the dynamic signature for the second GENERATE AC command using the CDA flag
/// </summary>
public class AuthenticateCombinedData2Command
{
    #region Instance Values

    private readonly DataObjectListResult _CardRiskManagementDataObjectList1Result;
    private readonly DataObjectListResult _CardRiskManagementDataObjectList2Result;
    private readonly GenerateAcCdaResponseMessage _GenerateAcCdaResponseMessage;
    private readonly DecodedIccPublicKeyCertificate _IccPublicKeyCertificate;
    private readonly DataObjectListResult _ProcessingOptionsDataObjectListResult;
    private readonly UnpredictableNumber _UnpredictableNumber;

    #endregion

    #region Constructor

    public AuthenticateCombinedData2Command(
        DecodedIccPublicKeyCertificate iccPublicKeyCertificate,
        UnpredictableNumber unpredictableNumber,
        DataObjectListResult processingOptionsDataObjectListResult,
        DataObjectListResult cardRiskManagementDataObjectList1Result,
        DataObjectListResult cardRiskManagementDataObjectList2Result,
        GenerateAcCdaResponseMessage generateAcCdaResponseMessage,
        SignedDynamicApplicationData signedDynamicApplicationData)
    {
        _IccPublicKeyCertificate = iccPublicKeyCertificate;
        _UnpredictableNumber = unpredictableNumber;
        _ProcessingOptionsDataObjectListResult = processingOptionsDataObjectListResult;
        _CardRiskManagementDataObjectList1Result = cardRiskManagementDataObjectList1Result;
        _CardRiskManagementDataObjectList2Result = cardRiskManagementDataObjectList2Result;
        _GenerateAcCdaResponseMessage = generateAcCdaResponseMessage;
    }

    #endregion

    #region Instance Members

    public GenerateAcCdaResponseMessage GetGenerateAcCdaResponseMessage() => _GenerateAcCdaResponseMessage;
    public DecodedIccPublicKeyCertificate GetIccPublicKeyCertificate() => _IccPublicKeyCertificate;

    public SignedDynamicApplicationData GetSignedDynamicApplicationData() =>
        _GenerateAcCdaResponseMessage.GetSignedDynamicApplicationData();

    public byte[] GetTransactionDataHashCodeInput(BerCodec codec)
    {
        List<byte> buffer = new();

        TagLengthValue[]? pdolResult = _ProcessingOptionsDataObjectListResult.AsTagLengthValueArray();
        TagLengthValue[]? cdolResult = _CardRiskManagementDataObjectList1Result.AsTagLengthValueArray();
        TagLengthValue[]? cdo2Result = _CardRiskManagementDataObjectList2Result.AsTagLengthValueArray();

        for (int i = 0; i < pdolResult.Length; i++)
            buffer.AddRange(pdolResult[i].GetValue());

        for (int i = 0; i < cdolResult.Length; i++)
            buffer.AddRange(cdolResult[i].GetValue());

        for (int i = 0; i < cdolResult.Length; i++)
            buffer.AddRange(cdo2Result[i].GetValue());

        buffer.AddRange(_GenerateAcCdaResponseMessage.GetTransactionDataHashData(codec));

        return buffer.ToArray();
    }

    public UnpredictableNumber GetUnpredictableNumber() => _UnpredictableNumber;

    #endregion
}