using Play.Testing.Infrastructure.AutoFixture;

namespace Play.Testing.Emv;

internal class EmvSpecimenBuilderFactory : SpecimenBuilderFactory
{
    #region Static Metadata

    public static readonly AmountAuthorizedNumericBuilder AmountAuthorizedNumericBuilder;

    #endregion

    #region Constructor

    public EmvSpecimenBuilderFactory() : base(CreateSpecimenBuilders())
    { }

    #endregion

    #region Instance Members

    #region Instance Members

    public static List<SpecimenBuilder> CreateSpecimenBuilders()
    {
        // Add upstream builders
        List<SpecimenBuilder> currentModuleBuilders = TestingSpecimenBuilderFactory.CreateSpecimenBuilders();

        // Add context specific SpecimenBuilders here
        currentModuleBuilders.AddRange(new List<SpecimenBuilder>()
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
            new StatusOutcomesBuilder(),
            new TerminalCategoryCodeBuilder(),
            new TerminalVerificationResultCodesBuilder(),
            new TransactionTypeBuilder(),
            new ValueQualifierBuilder(),
            new DirectoryEntryBuilder(),
            new FileControlInformationAdfBuilder(),
            new ProcessingOptionsDataObjectListBuilder(),
            new FileControlInformationIssuerDiscretionaryDataAdfBuilder()
        });

        return currentModuleBuilders;

        #endregion
    }

    #endregion
}