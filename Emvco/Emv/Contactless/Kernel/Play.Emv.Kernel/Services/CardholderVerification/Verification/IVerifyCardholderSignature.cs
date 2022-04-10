using Play.Emv.Ber;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholderSignature
{
    #region Instance Members

    public CvmCode Process();

    #endregion
}