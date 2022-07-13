using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class PosCardholderInteractionInformationTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContents = { 3, 5, 3 };

    public PosCardholderInteractionInformationTestTlv() : base(_DefaultContents) { }

    public PosCardholderInteractionInformationTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => PosCardholderInteractionInformation.Tag;
}
