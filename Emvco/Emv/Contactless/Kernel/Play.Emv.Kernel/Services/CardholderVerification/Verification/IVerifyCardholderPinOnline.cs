using Play.Emv.Ber;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholderPinOnline
{
    #region Instance Members

    public CvmCode Process(KernelDatabase database);

    #endregion
}