using Play.Emv.Ber;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholderSignature
{
    #region Instance Members

    public CvmCode Process();

    #endregion
}