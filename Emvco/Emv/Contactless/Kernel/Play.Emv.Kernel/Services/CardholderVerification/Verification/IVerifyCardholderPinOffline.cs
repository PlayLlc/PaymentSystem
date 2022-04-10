using Play.Emv.Ber;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholderPinOffline
{
    #region Instance Members

    public CvmCode Process(KernelDatabase database);

    #endregion
}