using AutoFixture.Kernel;

using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Constructed;

namespace Play.Testing.Emv.AutoFixture.Builders.Templates;

public class FileControlInformationProprietaryDdfBuilder : ConstructedValueSpecimenBuilder<FileControlInformationProprietaryDdf>
{
    public static readonly SpecimenBuilderId Id = new(nameof(FileControlInformationProprietaryDdf));
    private static readonly FileControlInformationProprietaryDdfTestTlv _DefaultTestTlv = new FileControlInformationProprietaryDdfTestTlv();

    public FileControlInformationProprietaryDdfBuilder()
        : base(new DefaultConstructedValueSpecimen<FileControlInformationProprietaryDdf>(FileControlInformationProprietaryDdf.Decode(_DefaultTestTlv.EncodeTagLengthValue()), _DefaultTestTlv.EncodeValue())) { }

    public FileControlInformationProprietaryDdfBuilder(DefaultConstructedValueSpecimen<FileControlInformationProprietaryDdf> value) : base(value)
    {
    }

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(FileControlInformationProprietaryDdf))
            return new NoSpecimen();

        return GetDefault();
    }

    public override SpecimenBuilderId GetId() => Id;
}
