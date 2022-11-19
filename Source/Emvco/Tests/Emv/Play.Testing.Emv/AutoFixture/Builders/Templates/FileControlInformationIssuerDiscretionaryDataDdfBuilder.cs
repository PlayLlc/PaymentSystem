using AutoFixture.Kernel;

using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Constructed;

namespace Play.Testing.Emv.AutoFixture.Builders.Templates;

public class FileControlInformationIssuerDiscretionaryDataDdfBuilder : ConstructedValueSpecimenBuilder<FileControlInformationIssuerDiscretionaryDataDdf>
{
    public static readonly SpecimenBuilderId Id = new(nameof(FileControlInformationIssuerDiscretionaryDataDdf));
    private static readonly FileControlInformationIssuerDiscretionaryDataDdfTestTlv _DefaultTestTlv = new FileControlInformationIssuerDiscretionaryDataDdfTestTlv();

    public FileControlInformationIssuerDiscretionaryDataDdfBuilder() :
        base(new DefaultConstructedValueSpecimen<FileControlInformationIssuerDiscretionaryDataDdf>(FileControlInformationIssuerDiscretionaryDataDdf.Decode(_DefaultTestTlv.EncodeTagLengthValue()), _DefaultTestTlv.EncodeValue()))
    { }

    public FileControlInformationIssuerDiscretionaryDataDdfBuilder(DefaultConstructedValueSpecimen<FileControlInformationIssuerDiscretionaryDataDdf> value) : base(value)
    {
    }

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(FileControlInformationIssuerDiscretionaryDataDdf))
            return new NoSpecimen();

        return GetDefault();
    }

    public override SpecimenBuilderId GetId() => Id;
}
