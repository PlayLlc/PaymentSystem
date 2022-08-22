using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholder
{
    #region Instance Members

    public CvmResults Process();

    #endregion
}