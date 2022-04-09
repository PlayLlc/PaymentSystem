using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;

using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders.Specimens;

namespace Play.Testing.Infrastructure.AutoFixture.FixtureFactories;

public class DefaultFixtureFactory
{
    #region Static Metadata

    private static readonly List<ISpecimenBuilder> _SpecimenBuilders;

    #endregion

    #region Constructor

    static DefaultFixtureFactory()
    {
        SpecimenForeman builder = new();
        builder.Build(RegisteredApplicationProviderIndicatorBuilder.Id);

        _SpecimenBuilders = builder.Complete();
    }

    #endregion

    #region Instance Members

    /// <exception cref="NotSupportedException"></exception>
    public static IFixture Create()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());

        foreach (ISpecimenBuilder item in _SpecimenBuilders)
            fixture.Customizations.Add(item);

        return fixture;
    }

    #endregion
}