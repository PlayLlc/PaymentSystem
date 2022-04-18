using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;

using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;

namespace Play.Testing.Infrastructure.AutoFixture;

public class TestingFixture : CustomFixture
{
    #region Instance Members

    /// <exception cref="NotSupportedException"></exception>
    public override IFixture Create()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        TestingSpecimenBuilderFactory factory = new();
        SetupCustomConstructors(factory);
        CustomizeFixture(fixture, factory);

        return fixture;
    }

    protected override void SetupCustomConstructors(SpecimenBuilderFactory factory)
    {
        //
        factory.Build(RegisteredApplicationProviderIndicatorSpecimenBuilder.Id);
        factory.Build(Alpha2LanguageCodeBuilder.Id);
        factory.Build(EnumObjectBuilder.Id);
        factory.Build(Alpha3CurrencyCodeBuilder.Id);
        factory.Build(Alpha3LanguageCodeBuilder.Id);
        factory.Build(DateTimeUtcBuilder.Id);
        factory.Build(NumericCurrencyCodeBuilder.Id);
        factory.Build(ShortDateBuilder.Id);
        factory.Build(DateRangeBuilder.Id);
    }

    protected override void CustomizeFixture(IFixture fixture, SpecimenBuilderFactory factory)
    {
        foreach (ISpecimenBuilder item in factory.Create())
            fixture.Customizations.Add(item);
    }

    #endregion
}