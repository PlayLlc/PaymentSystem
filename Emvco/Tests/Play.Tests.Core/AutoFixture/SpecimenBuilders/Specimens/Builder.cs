using AutoFixture.Kernel;

namespace Play.Emv.Ber.TestData.AutoFixture;

internal abstract class Builder : ISpecimenBuilder
{
    #region Instance Members

    public abstract SpecimenBuilderId GetId();
    public abstract object Create(object request, ISpecimenContext context);

    #endregion
}