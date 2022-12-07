using AutoFixture.Kernel;

using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.AutoFixture.Builders.DataElements;

public class TerminalTransactionQualifiersBuilder : PrimitiveValueSpecimenBuilder<TerminalTransactionQualifiers>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(TerminalTransactionQualifiersBuilder));

    private static byte[] GetContentOctets() =>
    new byte[]
    {
            0x61, 0x62, 0x63, 0x64,
    };

    #endregion

    #region Constructor

    public TerminalTransactionQualifiersBuilder() : base(new DefaultPrimitiveValueSpecimen<TerminalTransactionQualifiers>(TerminalTransactionQualifiers.Decode(GetContentOctets().AsSpan()), GetContentOctets())) { }

    public TerminalTransactionQualifiersBuilder(DefaultPrimitiveValueSpecimen<TerminalTransactionQualifiers> value) : base(value)
    {
    }

    #endregion

    public override SpecimenBuilderId GetId() => Id;

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(TerminalTransactionQualifiers))
            return new NoSpecimen();

        return TerminalTransactionQualifiers.Decode(GetContentOctets().AsSpan());
    }
}
