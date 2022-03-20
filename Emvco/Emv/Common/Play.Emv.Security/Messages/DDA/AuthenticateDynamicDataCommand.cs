using Play.Emv.Ber.DataElements;
using Play.Emv.Security.Certificates.Icc;
using Play.Encryption.Certificates;

namespace Play.Emv.Security.Messages.DDA;

public class AuthenticateDynamicDataCommand
{
    #region Instance Values

    private readonly DataObjectListResult _DataObjectListResult;
    private readonly DecodedIccPublicKeyCertificate _IccPublicKeyCertificate;
    private readonly SignedDynamicApplicationData _SignedDynamicApplicationData;

    #endregion

    #region Constructor

    public AuthenticateDynamicDataCommand(
        DecodedIccPublicKeyCertificate iccPublicKeyCertificate,
        DataObjectListResult dynamicDataObjectListResult,
        SignedDynamicApplicationData signedDynamicApplicationData

        // 
    )
    {
        _IccPublicKeyCertificate = iccPublicKeyCertificate;
        _DataObjectListResult = dynamicDataObjectListResult;
        _SignedDynamicApplicationData = signedDynamicApplicationData;
    }

    #endregion

    #region Instance Members

    public DataObjectListResult GetDataObjectListResult() => _DataObjectListResult;
    public PublicKeyCertificate GetIssuerPublicKeyCertificate() => _IccPublicKeyCertificate;
    public SignedDynamicApplicationData GetSignedDynamicApplicationData() => _SignedDynamicApplicationData;

    #endregion
}