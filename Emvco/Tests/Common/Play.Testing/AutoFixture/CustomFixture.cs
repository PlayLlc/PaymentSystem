using AutoFixture;

namespace Play.Testing;

public abstract class CustomFixture
{
    #region Instance Members

    public abstract IFixture Create();
    protected abstract void SetupCustomConstructors(SpecimenBuilderFactory factory);
    protected abstract void CustomizeFixture(IFixture fixture, SpecimenBuilderFactory factory);

    #endregion
}