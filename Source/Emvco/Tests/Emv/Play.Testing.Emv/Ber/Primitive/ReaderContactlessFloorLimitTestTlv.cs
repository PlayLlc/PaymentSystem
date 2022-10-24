using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ReaderContactlessFloorLimitTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12, 22, 31, 44, 15, 64 };

    public ReaderContactlessFloorLimitTestTlv() : base(_DefaultContentOctets) { }

    public ReaderContactlessFloorLimitTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ReaderContactlessFloorLimit.Tag;
}
