using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel.Services.ApplicationCryptograms;

public interface IPrepareGenerateApplicationCryptogram
{
    #region Instance Members

    public GenerateApplicationCryptogramRequest Process();

    #endregion
}