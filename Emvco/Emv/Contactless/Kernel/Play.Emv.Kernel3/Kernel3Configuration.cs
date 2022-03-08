using Play.Emv.DataElements;

namespace Play.Emv.Kernel3;

public record Kernel3Configuration : KernelConfiguration
{
    #region Constructor

    protected Kernel3Configuration(KernelConfiguration original) : base(original)
    {
        throw new NotImplementedException();
    }

    #endregion
}