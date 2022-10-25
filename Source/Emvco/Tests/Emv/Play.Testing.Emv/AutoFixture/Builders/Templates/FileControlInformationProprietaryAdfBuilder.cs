using AutoFixture.Kernel;

using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Constructed;

namespace Play.Testing.Emv.AutoFixture.Builders.Templates;

public class FileControlInformationProprietaryAdfBuilder : ConstructedValueSpecimenBuilder<FileControlInformationProprietaryAdf>
{
    public static readonly SpecimenBuilderId Id = new(nameof(FileControlInformationProprietaryAdfBuilder));
    private static readonly FileControlInformationProprietaryAdfTestTlv _DefaultTestTlv = new FileControlInformationProprietaryAdfTestTlv();

    public FileControlInformationProprietaryAdfBuilder() : base(
        new DefaultConstructedValueSpecimen<FileControlInformationProprietaryAdf>(FileControlInformationProprietaryAdf.Decode(_DefaultTestTlv.EncodeTagLengthValue()), _DefaultTestTlv.EncodeValue()))
    { }

    public FileControlInformationProprietaryAdfBuilder(DefaultConstructedValueSpecimen<FileControlInformationProprietaryAdf> value) : base(value)
    { }

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(FileControlInformationProprietaryAdf))
            return new NoSpecimen();

        return GetDefault();
    }

    public override SpecimenBuilderId GetId() => Id;
}
