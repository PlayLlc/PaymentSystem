using AutoFixture.Kernel;

using Play.Testing.Emv.Infrastructure.AutoFixture.Specimens;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders.Specimens;

namespace Play.Testing.Emv.Infrastructure.AutoFixture.FixtureFactories;

public class SpecimenForeman
{
    #region Static Metadata

    private static readonly Dictionary<SpecimenBuilderId, ISpecimenBuilder> _Map;

    #endregion

    #region Instance Values

    public readonly SpecimenBuilderId RegisteredApplicationProviderIndicatorSpecimen =
        RegisteredApplicationProviderIndicatorSpecimenBuilder.Id;

    public readonly SpecimenBuilderId AlternateInterfacePreferenceOutcome = AlternateInterfacePreferenceOutcomeBuilder.Id;
    public readonly SpecimenBuilderId CvmPerformedOutcome = CvmPerformedOutcomeBuilder.Id;
    public readonly SpecimenBuilderId OnlineResponseOutcome = OnlineResponseOutcomeBuilder.Id;
    public readonly SpecimenBuilderId PinBlock = PinBlockBuilder.Id;
    public readonly SpecimenBuilderId SdsSchemeIndicator = SdsSchemeIndicatorBuilder.Id;
    public readonly SpecimenBuilderId StartOutcome = StartOutcomeBuilder.Id;
    public readonly SpecimenBuilderId Status = StatusBuilder.Id;
    public readonly SpecimenBuilderId StatusOutcome = StatusOutcomeBuilder.Id;
    public readonly SpecimenBuilderId TerminalCategoryCode = TerminalCategoryCodeBuilder.Id;
    public readonly SpecimenBuilderId ValueQualifier = ValueQualifierBuilder.Id;
    public readonly SpecimenBuilderId TransactionType = TransactionTypeBuilder.Id;
    public readonly SpecimenBuilderId CvmRule = CvmRuleBuilder.Id;
    public readonly SpecimenBuilderId MessageOnErrorIdentifiers = MessageOnErrorIdentifiersBuilder.Id;
    public readonly SpecimenBuilderId MessageTableEntry = MessageTableEntryBuilder.Id;
    public readonly SpecimenBuilderId TerminalVerificationResultCodes = TerminalVerificationResultCodesBuilder.Id;
    private readonly List<ISpecimenBuilder> _Buffer;

    #endregion

    #region Constructor

    static SpecimenForeman()
    {
        _Map = new Dictionary<SpecimenBuilderId, ISpecimenBuilder>()
        {
            {RegisteredApplicationProviderIndicatorSpecimenBuilder.Id, new RegisteredApplicationProviderIndicatorSpecimenBuilder()},
            {AlternateInterfacePreferenceOutcomeBuilder.Id, new AlternateInterfacePreferenceOutcomeBuilder()},
            {CvmPerformedOutcomeBuilder.Id, new CvmPerformedOutcomeBuilder()},
            {OnlineResponseOutcomeBuilder.Id, new OnlineResponseOutcomeBuilder()},
            {PinBlockBuilder.Id, new PinBlockBuilder()},
            {SdsSchemeIndicatorBuilder.Id, new SdsSchemeIndicatorBuilder()},
            {StartOutcomeBuilder.Id, new StartOutcomeBuilder()},
            {StatusBuilder.Id, new StatusBuilder()},
            {StatusOutcomeBuilder.Id, new StatusOutcomeBuilder()},
            {TerminalCategoryCodeBuilder.Id, new TerminalCategoryCodeBuilder()},
            {ValueQualifierBuilder.Id, new ValueQualifierBuilder()},
            {TransactionTypeBuilder.Id, new TransactionTypeBuilder()},
            {CvmRuleBuilder.Id, new CvmRuleBuilder()},
            {MessageOnErrorIdentifiersBuilder.Id, new MessageOnErrorIdentifiersBuilder()},
            {MessageTableEntryBuilder.Id, new MessageTableEntryBuilder()},
            {TerminalVerificationResultCodesBuilder.Id, new TerminalVerificationResultCodesBuilder()}
        };
    }

    public SpecimenForeman()
    {
        _Buffer = new List<ISpecimenBuilder>();
    }

    #endregion

    #region Instance Members

    public void Build(params SpecimenBuilderId[] map)
    {
        foreach (SpecimenBuilderId mapItem in map)
            _Buffer.Add(_Map[mapItem]);
    }

    public List<ISpecimenBuilder> Complete() => _Buffer;
    public void Clear() => _Buffer.Clear();

    #endregion
}