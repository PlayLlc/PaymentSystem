using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholder
{
    #region Instance Members

    public CvmCode Process(KernelDatabase database, params CardholderVerificationMethods[] cardholderVerificationMethods);

    #endregion
}