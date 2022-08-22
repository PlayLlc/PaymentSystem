using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class PunatcTrack2TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x9F, 0x36 };

    public PunatcTrack2TestTlv() : base(_DefaultContentOctets) { }

    public PunatcTrack2TestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => PunatcTrack2.Tag;
}
