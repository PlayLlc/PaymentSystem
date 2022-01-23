namespace Play.Emv.Terminal.Configuration.ApplicationDependent;

public interface IApplicationDependentConfigurationRepository
{
    #region Instance Members

    public bool TryGet(ApplicationIdentifier applicationIdentifier, out ApplicationDependentConfiguration result);

    #endregion
}