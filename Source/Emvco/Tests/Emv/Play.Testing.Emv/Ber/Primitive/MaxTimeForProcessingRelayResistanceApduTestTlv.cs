using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MaxTimeForProcessingRelayResistanceApduTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 11, 37 };

    public MaxTimeForProcessingRelayResistanceApduTestTlv() : base(_DefaultContentOctets) { }

    public MaxTimeForProcessingRelayResistanceApduTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => MaxTimeForProcessingRelayResistanceApdu.Tag;
}
