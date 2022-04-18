using AutoFixture.Kernel;

using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;

namespace Play.Testing.Infrastructure.AutoFixture;

/// <summary>
///     A builder that builds a list of custom <see cref="ISpecimenBuilder" /> implementations
/// </summary>
public class TestingSpecimenBuilderFactory : SpecimenBuilderFactory
{
    #region Constructor

    public TestingSpecimenBuilderFactory() : base(CreateSpecimenBuilders())
    { }

    #endregion

    #region Instance Members

    public static List<SpecimenBuilder> CreateSpecimenBuilders() => new() {new RegisteredApplicationProviderIndicatorSpecimenBuilder()};

    #endregion
}