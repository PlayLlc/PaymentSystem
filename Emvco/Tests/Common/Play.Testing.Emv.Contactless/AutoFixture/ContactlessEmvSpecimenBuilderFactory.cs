namespace Play.Testing.Emv.Contactless.AutoFixture;

public class ContactlessEmvSpecimenBuilderFactory : SpecimenBuilderFactory
{
    #region Constructor

    public ContactlessEmvSpecimenBuilderFactory() : base(SetupSpecimenBuilders())
    { }

    #endregion

    #region Instance Members

    private static List<SpecimenBuilder> SetupSpecimenBuilders() =>
        new()
        {
            new RegisteredApplicationProviderIndicatorSpecimenBuilder(),
            new AlternateInterfacePreferenceOutcomeBuilder(),
            new CvmPerformedOutcomeBuilder(),
            new OnlineResponseOutcomeBuilder(),
            new PinBlockBuilder(),
            new SdsSchemeIndicatorBuilder(),
            new StartOutcomeBuilder(),
            new StatusBuilder(),
            new StatusOutcomeBuilder(),
            new TerminalCategoryCodeBuilder(),
            new ValueQualifierBuilder(),
            new TransactionTypeBuilder(),
            new CvmRuleBuilder(),
            new MessageOnErrorIdentifiersBuilder(),
            new MessageTableEntryBuilder(),
            new TerminalVerificationResultCodesBuilder()
        };

    #endregion
}