using AutoFixture.Kernel;

using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.AutoFixture.Builders.DataElements;

public class TerminalIdentificationBuilder : PrimitiveValueSpecimenBuilder<TerminalIdentification>
{
    public static readonly SpecimenBuilderId Id = new(nameof(TerminalIdentificationBuilder));

    private static byte[] GetContentOctets() =>
        new byte[]
        {
            (byte)'t', (byte)'e', (byte)'s', (byte)'t', (byte)'b', (byte)'u', (byte)'i', (byte)'2',
        };

    public TerminalIdentificationBuilder() : base(new DefaultPrimitiveValueSpecimen<TerminalIdentification>(TerminalIdentification.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
    {
    }

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(TerminalIdentification))
            return new NoSpecimen();

        return GetDefault();
    }

    public override SpecimenBuilderId GetId() => Id;
}
