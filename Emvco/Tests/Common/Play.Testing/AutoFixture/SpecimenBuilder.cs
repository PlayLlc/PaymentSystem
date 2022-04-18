using AutoFixture.Kernel;

namespace Play.Testing;

public abstract class SpecimenBuilder : ISpecimenBuilder
{
    #region Static Metadata

    protected static Random _Random = new();

    #endregion

    #region Instance Members

    public abstract SpecimenBuilderId GetId();
    public abstract object Create(object request, ISpecimenContext context);

    #endregion
}