using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationUsageControlTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 21, 12 };

    public ApplicationUsageControlTestTlv() : base(_DefaultContentOctets) { }


    public ApplicationUsageControlTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ApplicationUsageControl.Tag;
}
