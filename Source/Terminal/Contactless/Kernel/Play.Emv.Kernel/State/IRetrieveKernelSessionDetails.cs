namespace Play.Emv.Kernel.State;

public interface IRetrieveKernelSessionDetails
{
    #region Instance Members

    public KernelSession GetKernelSession();

    #endregion
}