using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class NumericApplicationTransactionCounterTrack2TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x6E };

    public NumericApplicationTransactionCounterTrack2TestTlv() : base(_DefaultContentOctets) { }

    public NumericApplicationTransactionCounterTrack2TestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => NumericApplicationTransactionCounterTrack2.Tag;
}
