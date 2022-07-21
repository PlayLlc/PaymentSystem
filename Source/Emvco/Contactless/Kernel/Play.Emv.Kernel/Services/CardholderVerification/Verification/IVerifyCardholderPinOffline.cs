using Play.Emv.Ber;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholderPinOffline
{
    #region Instance Members

    public CvmCode Process(ITlvReaderAndWriter database);

    #endregion
}