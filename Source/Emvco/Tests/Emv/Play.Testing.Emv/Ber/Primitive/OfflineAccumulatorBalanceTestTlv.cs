using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class OfflineAccumulatorBalanceTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 40, 81, 38, 43, 81, 98 };

    public OfflineAccumulatorBalanceTestTlv() : base(_DefaultContentOctets) { }

    public OfflineAccumulatorBalanceTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => OfflineAccumulatorBalance.Tag;
}
