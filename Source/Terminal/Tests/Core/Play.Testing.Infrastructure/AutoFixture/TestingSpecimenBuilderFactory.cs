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

    public static List<SpecimenBuilder> CreateSpecimenBuilders() =>
        new()
        {
            new EnumObjectBuilder(),
            new Alpha2LanguageCodeBuilder(),
            new Alpha3CurrencyCodeBuilder(),
            new Alpha3LanguageCodeBuilder(),
            new DateTimeUtcBuilder(),
            new NumericCurrencyCodeBuilder(),
            new RegisteredApplicationProviderIndicatorSpecimenBuilder(),
            new ShortDateBuilder(),
            new DateRangeBuilder(),
            new PublicKeyExponentBuilder()
        };

    #endregion
}