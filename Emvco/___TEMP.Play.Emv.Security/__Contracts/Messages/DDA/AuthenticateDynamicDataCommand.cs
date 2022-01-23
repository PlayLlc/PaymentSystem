using ___TEMP.Play.Emv.Security.Certificates.Chip;
using ___TEMP.Play.Emv.Security.Cryptograms;

using Play.Ber.Emv.DataObjects;
using Play.Emv.DataElements;

namespace ___TEMP.Play.Emv.Security.Authentications.Dynamic.DynamicDataAuthentication;

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

    public DataObjectListResult GetDataObjectListResult()
    {
        return _DataObjectListResult;
    }

    public PublicKeyCertificate GetIssuerPublicKeyCertificate()
    {
        return _IccPublicKeyCertificate;
    }

    public SignedDynamicApplicationData GetSignedDynamicApplicationData()
    {
        return _SignedDynamicApplicationData;
    }

    #endregion
}