using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MinTimeForProcessingRelayResistanceApduTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 9, 64 };

    public MinTimeForProcessingRelayResistanceApduTestTlv() : base(_DefaultContentOctets) { }

    public MinTimeForProcessingRelayResistanceApduTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => MinTimeForProcessingRelayResistanceApdu.Tag;
}
