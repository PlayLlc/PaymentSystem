using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;

using Play.Tests.Core.AutoFixture.SpecimenBuilders;
using Play.Tests.Core.AutoFixture.SpecimenBuilders.Specimens;

namespace Play.Tests.Core.AutoFixture.FixtureFactories;

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