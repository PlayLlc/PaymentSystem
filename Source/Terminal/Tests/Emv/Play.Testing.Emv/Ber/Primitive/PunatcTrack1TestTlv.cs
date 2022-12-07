using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class PunatcTrack1TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x9F, 0x36, 0x27, 0x09, 0x13, 0x6e };

    public PunatcTrack1TestTlv() : base(_DefaultContentOctets) { }

    public PunatcTrack1TestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => PunatcTrack1.Tag;
}
