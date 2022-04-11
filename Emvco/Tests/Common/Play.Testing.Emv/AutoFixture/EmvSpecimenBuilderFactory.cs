namespace Play.Testing.Emv;

internal class EmvSpecimenBuilderFactory : SpecimenBuilderFactory
{
    #region Constructor

    public EmvSpecimenBuilderFactory() : base(SetupSpecimenBuilders())
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