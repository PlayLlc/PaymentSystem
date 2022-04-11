using AutoFixture.Kernel;

namespace Play.Testing;

public abstract class SpecimenBuilder : ISpecimenBuilder
{
    #region Instance Members

    public abstract SpecimenBuilderId GetId();
    public abstract object Create(object request, ISpecimenContext context);

    #endregion
}