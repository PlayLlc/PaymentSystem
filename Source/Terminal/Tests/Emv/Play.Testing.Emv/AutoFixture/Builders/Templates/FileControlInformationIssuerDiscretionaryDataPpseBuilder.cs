using AutoFixture.Kernel;

using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Constructed;

namespace Play.Testing.Emv.AutoFixture.Builders.Templates;

public class FileControlInformationIssuerDiscretionaryDataPpseBuilder : ConstructedValueSpecimenBuilder<FileControlInformationIssuerDiscretionaryDataPpse>
{
    public static readonly SpecimenBuilderId Id = new(nameof(FileControlInformationIssuerDiscretionaryDataPpse));
    private static readonly FileControlInformationIssuerDiscretionaryDataPpseTestTlv _DefaultTestTlv = new FileControlInformationIssuerDiscretionaryDataPpseTestTlv();

    public FileControlInformationIssuerDiscretionaryDataPpseBuilder()
        : base(new DefaultConstructedValueSpecimen<FileControlInformationIssuerDiscretionaryDataPpse>(FileControlInformationIssuerDiscretionaryDataPpse.Decode(_DefaultTestTlv.EncodeTagLengthValue()), _DefaultTestTlv.EncodeValue())) { }

    public FileControlInformationIssuerDiscretionaryDataPpseBuilder(DefaultConstructedValueSpecimen<FileControlInformationIssuerDiscretionaryDataPpse> value) : base(value)
    {
    }

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(FileControlInformationIssuerDiscretionaryDataPpse))
            return new NoSpecimen();

        return GetDefault();
    }

    public override SpecimenBuilderId GetId() => Id;
}
