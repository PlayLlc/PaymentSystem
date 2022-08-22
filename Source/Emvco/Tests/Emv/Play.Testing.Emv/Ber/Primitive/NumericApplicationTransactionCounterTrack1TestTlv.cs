using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class NumericApplicationTransactionCounterTrack1TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x9F };

    public NumericApplicationTransactionCounterTrack1TestTlv() : base(_DefaultContentOctets) { }

    public NumericApplicationTransactionCounterTrack1TestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => NumericApplicationTransactionCounterTrack1.Tag;
}
