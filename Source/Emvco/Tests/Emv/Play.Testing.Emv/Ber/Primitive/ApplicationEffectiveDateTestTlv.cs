using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationEffectiveDateTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 22, 07, 07 };

    public ApplicationEffectiveDateTestTlv() : base(_DefaultContentOctets) { }

    public ApplicationEffectiveDateTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    public override Tag GetTag() => ApplicationEffectiveDate.Tag;
}

