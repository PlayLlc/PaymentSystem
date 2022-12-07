using AutoFixture.Kernel;

using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Constructed;

namespace Play.Testing.Emv.AutoFixture.Builders.Templates;

public class FileControlInformationDdfBuilder : ConstructedValueSpecimenBuilder<FileControlInformationDdf>
{
    public static readonly SpecimenBuilderId Id = new(nameof(FileControlInformationDdf));
    private static readonly FileControlInformationDdfTestTlv _DefaultTestTlv = new FileControlInformationDdfTestTlv();

    public FileControlInformationDdfBuilder()
        : base(new DefaultConstructedValueSpecimen<FileControlInformationDdf>(FileControlInformationDdf.Decode(_DefaultTestTlv.EncodeTagLengthValue()), _DefaultTestTlv.EncodeValue())) { }

    public FileControlInformationDdfBuilder(DefaultConstructedValueSpecimen<FileControlInformationDdf> value) : base(value)
    {
    }

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(FileControlInformationDdf))
            return new NoSpecimen();

        return GetDefault();
    }

    public override SpecimenBuilderId GetId() => Id;
}
