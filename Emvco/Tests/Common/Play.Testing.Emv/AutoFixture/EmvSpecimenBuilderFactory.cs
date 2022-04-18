using Play.Testing.Infrastructure.AutoFixture;

namespace Play.Testing.Emv;

public class EmvSpecimenBuilderFactory : SpecimenBuilderFactory
{
    #region Constructor

    public EmvSpecimenBuilderFactory() : base(CreateSpecimenBuilders())
    { }

    #endregion

    #region Instance Members

    public static List<SpecimenBuilder> CreateSpecimenBuilders()
    {
        List<SpecimenBuilder> downStreamBuilders = TestingSpecimenBuilderFactory.CreateSpecimenBuilders();
        HashSet<SpecimenBuilder> currentModuleBuilders = new()
        {
            new AlternateInterfacePreferenceOutcomeBuilder(),
            new CertificateSerialNumberBuilder(),
            new CvmPerformedOutcomeBuilder(),
            new CvmRuleBuilder(),
            new MessageOnErrorIdentifiersBuilder(),
            new MessageTableEntryBuilder(),
            new OnlineResponseOutcomeBuilder(),
            new PinBlockBuilder(),
            new SdsSchemeIndicatorBuilder(),
            new StartOutcomeBuilder(),
            new StatusBuilder(),
            new StatusOutcomeBuilder(),
            new TerminalCategoryCodeBuilder(),
            new TerminalVerificationResultCodesBuilder(),
            new TransactionTypeBuilder(),
            new ValueQualifierBuilder()
        };

        return currentModuleBuilders.Concat(downStreamBuilders).ToList();
    }

    #endregion
}