using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.Selection;

public interface ISelectCardholderVerificationMethod
{
    #region Instance Members

    public void Process(KernelDatabase database);

    #endregion
}