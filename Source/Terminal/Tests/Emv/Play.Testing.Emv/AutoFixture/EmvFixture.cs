﻿using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Emv.Security;
using Play.Encryption.Certificates;
using Play.Encryption.Ciphers.Hashing;
using Play.Testing.Emv.AutoFixture.Builders.DataElements;
using Play.Testing.Emv.AutoFixture.Builders.Templates;
using Play.Testing.Emv.Ber.Constructed;
using Play.Testing.Extensions;
using Play.Testing.Infrastructure.AutoFixture;

namespace Play.Testing.Emv;

public class EmvFixture : TestingFixture
{
    #region Instance Members

    public override IFixture Create()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        EmvSpecimenBuilderFactory builder = new();

        SetupCustomBuilders(builder);
        CustomizeFixture(fixture, builder);

        return fixture;
    }

    protected override void SetupCustomBuilders(SpecimenBuilderFactory factory)
    {
        // Setup upstream builders
        base.SetupCustomBuilders(factory);

        //Globalization
        //factory.Build(NumericCountryCodeBuilder.Id);
        //factory.Build(Alpha2CountryCodeBuilder.Id);

        // Setup custom builder specific to this module's context
        factory.Build(AlternateInterfacePreferenceOutcomeBuilder.Id);
        factory.Build(CertificateSerialNumberBuilder.Id);
        factory.Build(CvmPerformedOutcomeBuilder.Id);
        factory.Build(CvmRuleBuilder.Id);
        factory.Build(DedicatedFileNameBuilder.Id);
        factory.Build(MessageOnErrorIdentifiersBuilder.Id);
        factory.Build(MessageTableEntryBuilder.Id);
        factory.Build(OnlineResponseOutcomeBuilder.Id);
        factory.Build(PinBlockBuilder.Id);
        factory.Build(SdsSchemeIndicatorBuilder.Id);
        factory.Build(StartOutcomeBuilder.Id);
        factory.Build(StatusBuilder.Id);
        factory.Build(StatusOutcomesBuilder.Id);
        factory.Build(TerminalCategoryCodeBuilder.Id);
        factory.Build(TerminalVerificationResultCodesBuilder.Id);
        factory.Build(TransactionTypeBuilder.Id);
        factory.Build(ValueQualifierBuilder.Id);
        factory.Build(DirectoryEntryBuilder.Id);
        factory.Build(FileControlInformationAdfBuilder.Id);
        factory.Build(ProcessingOptionsDataObjectListBuilder.Id);
        factory.Build(FileControlInformationIssuerDiscretionaryDataAdfBuilder.Id);

        factory.Build(ApplicationPanBuilder.Id);
        factory.Build(AmountAuthorizedNumericBuilder.Id);
        factory.Build(AmountOtherNumericBuilder.Id);
        factory.Build(ApplicationDedicatedFileNameBuilder.Id);
        factory.Build(ApplicationExpirationDateBuilder.Id);
        factory.Build(ApplicationFileLocatorBuilder.Id);
        factory.Build(ApplicationInterchangeProfileBuilder.Id);
        factory.Build(ApplicationLabelBuilder.Id);
        factory.Build(ApplicationPreferredNameBuilder.Id);
        factory.Build(ApplicationPriorityIndicatorBuilder.Id);
        factory.Build(CardholderNameBuilder.Id);
        factory.Build(CvmResultsBuilder.Id);
        factory.Build(IssuerIdentificationNumberBuilder.Id);
        factory.Build(KernelIdentifierBuilder.Id);
        factory.Build(MerchantIdentifierBuilder.Id);
        factory.Build(TransactionDateBuilder.Id);
        factory.Build(FileControlInformationProprietaryAdfBuilder.Id);
        factory.Build(FileControlInformationProprietaryDdfBuilder.Id);
        factory.Build(FileControlInformationIssuerDiscretionaryDataPpseBuilder.Id);
        factory.Build(LanguagePreferenceBuilder.Id);
        factory.Build(InterfaceDeviceSerialNumberBuilder.Id);
        factory.Build(TerminalIdentificationBuilder.Id);
        factory.Build(TerminalTransactionQualifiersBuilder.Id);
        factory.Build(ResponseMessageTemplateFormat2Builder.Id);
    }

    #region Customize Fixture

    protected override void CustomizeFixture(IFixture fixture, SpecimenBuilderFactory factory)
    {
        base.CustomizeFixture(fixture, factory);

        foreach (ISpecimenBuilder item in factory.Create())
            fixture.Customizations.Add(item);

        CustomizePrimitives(fixture);
        CustomizeTemplates(fixture);
        CustomizeEnumObjects(fixture);
        CustomizeObjects(fixture);
    }

    #endregion

    #region Customize Primitive Values

    private static void CustomizePrimitives(IFixture fixture)
    {
        fixture.Register(() => new ProcessingOptionsDataObjectList(EmvCodec.GetCodec().DecodeTagLengths(new byte[]
        {
            0x9F, 0x66, 0x04, 0x9F, 0x02, 0x06, 0x9F, 0x03, 0x06, 0x9F,
            0x1A, 0x02, 0x95, 0x05, 0x5F, 0x2A, 0x02, 0x9A, 0x03, 0x9C,
            0x01, 0x9F, 0x37, 0x04, 0x9F, 0x4E, 0x14
        })));
    }

    #endregion

    #region Customize Constructed Values

    private static void CustomizeTemplates(IFixture fixture)
    {
        fixture.Register(() => FileControlInformationPpse.Decode(new byte[]
        {
            0x6F, 0x4D, 0x84, 0x0E, 0x32, 0x50, 0x41, 0x59, 0x2E, 0x53,
            0x59, 0x53, 0x2E, 0x44, 0x44, 0x46, 0x30, 0x31, 0xA5, 0x3B,
            0xBF, 0x0C, 0x38, 0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00,
            0x00, 0x03, 0x10, 0x10, 0x87, 0x01, 0x01, 0x9F, 0x2A, 0x01,
            0x03, 0x42, 0x03, 0x40, 0x81, 0x38, 0x5F, 0x55, 0x02, 0x55,
            0x53, 0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98,
            0x08, 0x40, 0x87, 0x01, 0x02, 0x9F, 0x2A, 0x01, 0x03, 0x42,
            0x03, 0x40, 0x81, 0x38, 0x5F, 0x55, 0x02, 0x55, 0x53, 0x90,
            0x00
        }.AsMemory()));

        fixture.Register(() => FileControlInformationProprietaryPpse.Decode(new byte[]
        {
            0xA5, 0x3B, 0xBF, 0x0C, 0x38, 0x61, 0x1A, 0x4F, 0x07, 0xA0,
            0x00, 0x00, 0x00, 0x03, 0x10, 0x10, 0x87, 0x01, 0x01, 0x9F,
            0x2A, 0x01, 0x03, 0x42, 0x03, 0x40, 0x81, 0x38, 0x5F, 0x55,
            0x02, 0x55, 0x53, 0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00,
            0x00, 0x98, 0x08, 0x40, 0x87, 0x01, 0x02, 0x9F, 0x2A, 0x01,
            0x03, 0x42, 0x03, 0x40, 0x81, 0x38, 0x5F, 0x55, 0x02, 0x55,
            0x53
        }.AsMemory()));

        fixture.Register(() => FileControlInformationIssuerDiscretionaryDataPpse.Decode(new byte[]
        {
            0xBF, 0x0C, 0x38, 0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00,
            0x00, 0x03, 0x10, 0x10, 0x87, 0x01, 0x01, 0x9F, 0x2A, 0x01,
            0x03, 0x42, 0x03, 0x40, 0x81, 0x38, 0x5F, 0x55, 0x02, 0x55,
            0x53, 0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98,
            0x08, 0x40, 0x87, 0x01, 0x02, 0x9F, 0x2A, 0x01, 0x03, 0x42,
            0x03, 0x40, 0x81, 0x38, 0x5F, 0x55, 0x02, 0x55, 0x53
        }.AsMemory()));

        //fixture.Register(() => DirectoryEntry.Decode(new byte[]
        //{
        //    0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10,
        //    0x10, 0x87, 0x01, 0x01, 0x9F, 0x2A, 0x01, 0x03, 0x42, 0x03,
        //    0x40, 0x81, 0x38, 0x5F, 0x55, 0x02, 0x55, 0x53
        //}.AsMemory()));

        fixture.Register(() => ProcessingOptions.Decode(new ProcessingOptionsTestTlv().EncodeTagLengthValue().AsMemory()));
    }

    #endregion

    #region Customize Enum Objects

    protected static void CustomizeEnumObjects(IFixture fixture)
    {
        fixture.Register(() => HashAlgorithmIndicators.Sha1);
        fixture.Register(() => PublicKeyAlgorithmIndicators.Rsa);
    }

    #endregion

    #region Custimize Objects

    private static void CustomizeObjects(IFixture fixture)
    {
        fixture.RegisterCollections<CaPublicKeyCertificate>();

        //fixture.RegisterSetOf<DirectoryEntry>();
    }

    #endregion

    #endregion

    #region Primitive Value Builders

    public static readonly AmountAuthorizedNumericBuilder AmountAuthorizedNumericBuilder = EmvSpecimenBuilderFactory.AmountAuthorizedNumericBuilder;
    public static readonly AmountOtherNumericBuilder AmountOtherNumericBuilder = EmvSpecimenBuilderFactory.AmountOtherNumericBuilder;

    public static readonly ApplicationDedicatedFileNameBuilder ApplicationDedicatedFileNameBuilder =
        EmvSpecimenBuilderFactory.ApplicationDedicatedFileNameBuilder;

    public static readonly ApplicationExpirationDateBuilder ApplicationExpirationDateBuilder = EmvSpecimenBuilderFactory.ApplicationExpirationDateBuilder;
    public static readonly ApplicationFileLocatorBuilder ApplicationFileLocatorBuilder = EmvSpecimenBuilderFactory.ApplicationFileLocatorBuilder;

    public static readonly ApplicationInterchangeProfileBuilder ApplicationInterchangeProfileBuilder =
        EmvSpecimenBuilderFactory.ApplicationInterchangeProfileBuilder;

    public static readonly ApplicationLabelBuilder ApplicationLabelBuilder = EmvSpecimenBuilderFactory.ApplicationLabelBuilder;
    public static readonly ApplicationPreferredNameBuilder ApplicationPreferredNameBuilder = EmvSpecimenBuilderFactory.ApplicationPreferredNameBuilder;

    public static readonly ApplicationPriorityIndicatorBuilder ApplicationPriorityIndicatorBuilder =
        EmvSpecimenBuilderFactory.ApplicationPriorityIndicatorBuilder;

    public static readonly CardholderNameBuilder CardholderNameBuilder = EmvSpecimenBuilderFactory.CardholderNameBuilder;
    public static readonly CvmResultsBuilder CvmResultsBuilder = EmvSpecimenBuilderFactory.CvmResultsBuilder;
    public static readonly DedicatedFileNameBuilder DedicatedFileNameBuilder = EmvSpecimenBuilderFactory.DedicatedFileNameBuilder;
    public static readonly IssuerIdentificationNumberBuilder IssuerIdentificationNumberBuilder = EmvSpecimenBuilderFactory.IssuerIdentificationNumberBuilder;
    public static readonly KernelIdentifierBuilder KernelIdentifierBuilder = EmvSpecimenBuilderFactory.KernelIdentifierBuilder;
    public static readonly MerchantIdentifierBuilder MerchantIdentifierBuilder = EmvSpecimenBuilderFactory.MerchantIdentifierBuilder;
    public static readonly MerchantNameAndLocationBuilder MerchantNameAndLocationBuilder = EmvSpecimenBuilderFactory.MerchantNameAndLocationBuilder;

    public static readonly ProcessingOptionsDataObjectListBuilder ProcessingOptionsDataObjectListBuilder =
        EmvSpecimenBuilderFactory.ProcessingOptionsDataObjectListBuilder;

    public static readonly TransactionDateBuilder TransactionDateBuilder = EmvSpecimenBuilderFactory.TransactionDateBuilder;
    public static readonly TransactionTypeBuilder TransactionTypeBuilder = EmvSpecimenBuilderFactory.TransactionTypeBuilder;

    #endregion

    #region Constructed Value Builders

    public static readonly DirectoryEntryBuilder DirectoryEntryBuilder = EmvSpecimenBuilderFactory.DirectoryEntryBuilder;
    public static readonly FileControlInformationAdfBuilder FileControlInformationAdfBuilder = EmvSpecimenBuilderFactory.FileControlInformationAdfBuilder;

    public static readonly FileControlInformationIssuerDiscretionaryDataAdfBuilder FileControlInformationIssuerDiscretionaryDataAdfBuilder =
        EmvSpecimenBuilderFactory.FileControlInformationIssuerDiscretionaryDataAdfBuilder;

    public static readonly FileControlInformationProprietaryAdfBuilder FileControlInformationProprietaryAdfBuilder =
        EmvSpecimenBuilderFactory.FileControlInformationProprietaryAdfBuilder;

    public static readonly FileControlInformationProprietaryDdfBuilder FileControlInformationProprietaryDdfBuilder =
        EmvSpecimenBuilderFactory.FileControlInformationProprietaryDdfBuilder;

    public static readonly FileControlInformationDdfBuilder FileControlInformationDdfBuilder = EmvSpecimenBuilderFactory.FileControlInformationDdfBuilder;

    public static readonly FileControlInformationIssuerDiscretionaryDataPpseBuilder FileControlInformationIssuerDiscretionaryDataPpseBuilder =
        EmvSpecimenBuilderFactory.FileControlInformationIssuerDiscretionaryDataPpseBuilder;

    public static readonly ResponseMessageTemplateFormat2Builder ResponseMessageTemplateFormat2Builder =
        EmvSpecimenBuilderFactory.ResponseMessageTemplateFormat2Builder;

    #endregion
}