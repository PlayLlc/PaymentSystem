using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;

using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel2.Databases;
using Play.Testing.Infrastructure;

namespace Play.Testing.Emv.Contactless.AutoFixture;

public class DefaultEmvInfrastructureFixtureFactory // This is a FactoryOwner?
{
    #region Static Metadata

    private static readonly List<ISpecimenBuilder> _SpecimenBuilders;

    #endregion

    #region Constructor

    static DefaultEmvInfrastructureFixtureFactory()
    {
        Foreman builder = new();

        builder.Build(RegisteredApplicationProviderIndicatorSpecimenBuilder.Id);
        builder.Build(AlternateInterfacePreferenceOutcomeBuilder.Id);
        builder.Build(CvmPerformedOutcomeBuilder.Id);
        builder.Build(OnlineResponseOutcomeBuilder.Id);
        builder.Build(PinBlockBuilder.Id);
        builder.Build(SdsSchemeIndicatorBuilder.Id);
        builder.Build(StartOutcomeBuilder.Id);
        builder.Build(StatusBuilder.Id);
        builder.Build(StatusOutcomeBuilder.Id);
        builder.Build(TerminalCategoryCodeBuilder.Id);
        builder.Build(ValueQualifierBuilder.Id);
        builder.Build(TransactionTypeBuilder.Id);
        builder.Build(CvmRuleBuilder.Id);
        builder.Build(MessageOnErrorIdentifiersBuilder.Id);
        builder.Build(MessageTableEntryBuilder.Id);
        builder.Build(TerminalVerificationResultCodesBuilder.Id);

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

        SetupDatabase(fixture);

        return fixture;
    }

    private static void SetupDatabase(IFixture fixture)
    {
        fixture.Register(() => new KernelDatabase(fixture.Create<CertificateAuthorityDataset[]>(), new Kernel2PersistentValues(),
                                                  new Kernel2KnownObjects()));
        fixture.Freeze<KernelDatabase>();
    }

    #endregion
}