using Play.Testing.Infrastructure.AutoFixture;

namespace Play.Testing.Emv;

public class EmvSpecimenBuilderFactory : SpecimenBuilderFactory
{
    #region Constructor

    static EmvSpecimenBuilderFactory()
    {
        // Primitive Values
        AmountAuthorizedNumericBuilder = new AmountAuthorizedNumericBuilder();
        AmountOtherNumericBuilder = new AmountOtherNumericBuilder();
        ApplicationDedicatedFileNameBuilder = new ApplicationDedicatedFileNameBuilder();
        ApplicationExpirationDateBuilder = new ApplicationExpirationDateBuilder();
        ApplicationFileLocatorBuilder = new ApplicationFileLocatorBuilder();
        ApplicationInterchangeProfileBuilder = new ApplicationInterchangeProfileBuilder();
        ApplicationLabelBuilder = new ApplicationLabelBuilder();
        ApplicationPreferredNameBuilder = new ApplicationPreferredNameBuilder();
        ApplicationPriorityIndicatorBuilder = new ApplicationPriorityIndicatorBuilder();
        CardholderNameBuilder = new CardholderNameBuilder();
        CvmResultsBuilder = new CvmResultsBuilder();
        DedicatedFileNameBuilder = new DedicatedFileNameBuilder();
        IssuerIdentificationNumberBuilder = new IssuerIdentificationNumberBuilder();
        KernelIdentifierBuilder = new KernelIdentifierBuilder();
        MerchantIdentifierBuilder = new MerchantIdentifierBuilder();
        MerchantNameAndLocationBuilder = new MerchantNameAndLocationBuilder();
        ProcessingOptionsDataObjectListBuilder = new ProcessingOptionsDataObjectListBuilder();
        TransactionDateBuilder = new TransactionDateBuilder();
        TransactionTypeBuilder = new TransactionTypeBuilder();

        // Constructed Values
        DirectoryEntryBuilder = new DirectoryEntryBuilder();
        FileControlInformationAdfBuilder = new FileControlInformationAdfBuilder();
        FileControlInformationIssuerDiscretionaryDataAdfBuilder = new FileControlInformationIssuerDiscretionaryDataAdfBuilder();
        FileControlInformationIssuerDiscretionaryPpseBuilder = new FileControlInformationIssuerDiscretionaryPpseBuilder();
    }

    public EmvSpecimenBuilderFactory() : base(CreateSpecimenBuilders())
    { }

    #endregion

    #region Instance Members

    public static List<SpecimenBuilder> CreateSpecimenBuilders()
    {
        // Add upstream builders
        List<SpecimenBuilder> currentModuleBuilders = TestingSpecimenBuilderFactory.CreateSpecimenBuilders();

        // Add context specific SpecimenBuilders here
        currentModuleBuilders.AddRange(new List<SpecimenBuilder>
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
            new ValueQualifierBuilder(),
            TransactionTypeBuilder,
            DirectoryEntryBuilder,
            FileControlInformationAdfBuilder,
            ProcessingOptionsDataObjectListBuilder,
            FileControlInformationIssuerDiscretionaryDataAdfBuilder
        });

        return currentModuleBuilders;
    }

    #endregion

    #region Primitive Values

    public static readonly AmountAuthorizedNumericBuilder AmountAuthorizedNumericBuilder;
    public static readonly AmountOtherNumericBuilder AmountOtherNumericBuilder;
    public static readonly ApplicationDedicatedFileNameBuilder ApplicationDedicatedFileNameBuilder;
    public static readonly ApplicationExpirationDateBuilder ApplicationExpirationDateBuilder;
    public static readonly ApplicationFileLocatorBuilder ApplicationFileLocatorBuilder;
    public static readonly ApplicationInterchangeProfileBuilder ApplicationInterchangeProfileBuilder;
    public static readonly ApplicationLabelBuilder ApplicationLabelBuilder;
    public static readonly ApplicationPreferredNameBuilder ApplicationPreferredNameBuilder;
    public static readonly ApplicationPriorityIndicatorBuilder ApplicationPriorityIndicatorBuilder;
    public static readonly CardholderNameBuilder CardholderNameBuilder;
    public static readonly CvmResultsBuilder CvmResultsBuilder;
    public static readonly DedicatedFileNameBuilder DedicatedFileNameBuilder;
    public static readonly IssuerIdentificationNumberBuilder IssuerIdentificationNumberBuilder;
    public static readonly KernelIdentifierBuilder KernelIdentifierBuilder;
    public static readonly MerchantIdentifierBuilder MerchantIdentifierBuilder;
    public static readonly MerchantNameAndLocationBuilder MerchantNameAndLocationBuilder;
    public static readonly ProcessingOptionsDataObjectListBuilder ProcessingOptionsDataObjectListBuilder;
    public static readonly TransactionDateBuilder TransactionDateBuilder;
    public static readonly TransactionTypeBuilder TransactionTypeBuilder;

    #endregion

    #region Constructed Values

    public static readonly DirectoryEntryBuilder DirectoryEntryBuilder;
    public static readonly FileControlInformationAdfBuilder FileControlInformationAdfBuilder;
    public static readonly FileControlInformationIssuerDiscretionaryDataAdfBuilder FileControlInformationIssuerDiscretionaryDataAdfBuilder;
    public static readonly FileControlInformationIssuerDiscretionaryPpseBuilder FileControlInformationIssuerDiscretionaryPpseBuilder;

    #endregion
}