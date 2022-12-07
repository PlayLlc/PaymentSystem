using AutoFixture.Kernel;

using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.AutoFixture.Builders.DataElements;

public class InterfaceDeviceSerialNumberBuilder : PrimitiveValueSpecimenBuilder<InterfaceDeviceSerialNumber>
{
    public static readonly SpecimenBuilderId Id = new(nameof(InterfaceDeviceSerialNumberBuilder));

    private static byte[] GetContentOctets() =>
    new byte[]
    {
            (byte)'t', (byte)'e', (byte)'s', (byte)'t', (byte)'b', (byte)'u', (byte)'i', (byte)'2',
    };

    public InterfaceDeviceSerialNumberBuilder() : base(new DefaultPrimitiveValueSpecimen<InterfaceDeviceSerialNumber>(InterfaceDeviceSerialNumber.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
    {
    }

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(InterfaceDeviceSerialNumber))
            return new NoSpecimen();

        return InterfaceDeviceSerialNumber.Decode(GetContentOctets().AsSpan());
    }

    public override SpecimenBuilderId GetId() => Id;
}
