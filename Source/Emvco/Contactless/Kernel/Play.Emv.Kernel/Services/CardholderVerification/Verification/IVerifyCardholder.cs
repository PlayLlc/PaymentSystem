using Play.Emv.Ber;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholder
{
    #region Instance Members

    public CvmCode Process(ITlvReaderAndWriter database, params CardholderVerificationMethods[] cardholderVerificationMethods);

    #endregion
}