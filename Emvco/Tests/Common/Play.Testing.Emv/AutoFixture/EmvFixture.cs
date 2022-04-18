﻿using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;

using Play.Core.Extensions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Constructed;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;

namespace Play.Testing.Emv;

public class EmvFixture : CustomFixture
{
    #region Instance Members

    public override IFixture Create()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        EmvSpecimenBuilderFactory builder = new();

        SetupCustomConstructors(builder);
        CustomizeFixture(fixture, builder);

        return fixture;
    }

    protected override void SetupCustomConstructors(SpecimenBuilderFactory factory)
    {
        factory.Build(RegisteredApplicationProviderIndicatorSpecimenBuilder.Id);
        factory.Build(CertificateSerialNumberBuilder.Id);
        factory.Build(AlternateInterfacePreferenceOutcomeBuilder.Id);
        factory.Build(CvmPerformedOutcomeBuilder.Id);
        factory.Build(OnlineResponseOutcomeBuilder.Id);
        factory.Build(PinBlockBuilder.Id);
        factory.Build(SdsSchemeIndicatorBuilder.Id);
        factory.Build(StartOutcomeBuilder.Id);
        factory.Build(StatusBuilder.Id);
        factory.Build(StatusOutcomeBuilder.Id);
        factory.Build(TerminalCategoryCodeBuilder.Id);
        factory.Build(ValueQualifierBuilder.Id);
        factory.Build(TransactionTypeBuilder.Id);
        factory.Build(CvmRuleBuilder.Id);
        factory.Build(MessageOnErrorIdentifiersBuilder.Id);
        factory.Build(MessageTableEntryBuilder.Id);
        factory.Build(TerminalVerificationResultCodesBuilder.Id);
    }

    protected override void CustomizeFixture(IFixture fixture, SpecimenBuilderFactory factory)
    {
        foreach (ISpecimenBuilder item in factory.Create())
            fixture.Customizations.Add(item);
    }

    private static void CustomizePrimitives(IFixture fixture)
    {
        // TODO: Maybe add a builder pattern for registrations for specific use cases of each primitive so you can reuse them for the templates and custom fixtures?
        fixture.Register<ProcessingOptionsDataObjectList>(() => new ProcessingOptionsDataObjectList(new byte[]
        {
            0x9F, 0x66, 0x04, 0x9F, 0x02, 0x06, 0x9F, 0x03, 0x06, 0x9F,
            0x1A, 0x02, 0x95, 0x05, 0x5F, 0x2A, 0x02, 0x9A, 0x03, 0x9C,
            0x01, 0x9F, 0x37, 0x04, 0x9F, 0x4E, 0x14
        }.AsBigInteger()));
    }

    // TODO: This is just a cookbook example of how we would register these before a test. Each registration should probably move to its own method or builder class, or maybe we use the TestTlv objects and have methods for registering itself with different values for different scenarios
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

        fixture.Register(() => DirectoryEntry.Decode(new byte[]
        {
            0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10,
            0x10, 0x87, 0x01, 0x01, 0x9F, 0x2A, 0x01, 0x03, 0x42, 0x03,
            0x40, 0x81, 0x38, 0x5F, 0x55
        }.AsMemory()));

        fixture.Register(() => ProcessingOptions.Decode(new ProcessingOptionsTestTlv().EncodeTagLengthValue().AsMemory()));
    }

    #endregion
}