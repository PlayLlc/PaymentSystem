using AutoFixture.Kernel;

namespace Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders.Specimens;

internal abstract class Builder : ISpecimenBuilder
{
    #region Instance Members

    public abstract SpecimenBuilderId GetId();
    public abstract object Create(object request, ISpecimenContext context);

    #endregion
}