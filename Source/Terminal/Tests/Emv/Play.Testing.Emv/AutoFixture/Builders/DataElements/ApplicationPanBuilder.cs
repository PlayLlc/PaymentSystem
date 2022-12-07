using AutoFixture.Kernel;

using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.ValueTypes;

namespace Play.Testing.Emv.AutoFixture.Builders.DataElements;

public class ApplicationPanBuilder : PrimitiveValueSpecimenBuilder<ApplicationPan>
{
    public static readonly SpecimenBuilderId Id = new(nameof(ApplicationPanBuilder));

    public override SpecimenBuilderId GetId() => Id;

    public ApplicationPanBuilder() : base(
        new DefaultPrimitiveValueSpecimen<ApplicationPan>(ApplicationPan.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
    { }

    public ApplicationPanBuilder(DefaultPrimitiveValueSpecimen<ApplicationPan> value) : base(value)
    {}

    private static byte[] GetContentOctets() => new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };

    private static readonly TrackPrimaryAccountNumber _DefaultContent = new TrackPrimaryAccountNumber(new Nibble[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7,
    0x8, 0x9, 0x0, 0x1, 0x2, 0x3});

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(ApplicationPan))
            return new NoSpecimen();

        return new ApplicationPan(_DefaultContent);
    }
}
