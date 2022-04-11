using AutoFixture.Kernel;

using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;

namespace Play.Testing.Infrastructure.AutoFixture;

/// <summary>
///     A builder that builds a list of custom <see cref="ISpecimenBuilder" /> implementations
/// </summary>
internal class TestingSpecimenBuilderFactory : SpecimenBuilderFactory
{
    #region Constructor

    public TestingSpecimenBuilderFactory() : base(SetupSpecimenBuilders())
    { }

    #endregion

    #region Instance Members

    private static List<SpecimenBuilder> SetupSpecimenBuilders() => new() {new RegisteredApplicationProviderIndicatorSpecimenBuilder()};

    #endregion
}