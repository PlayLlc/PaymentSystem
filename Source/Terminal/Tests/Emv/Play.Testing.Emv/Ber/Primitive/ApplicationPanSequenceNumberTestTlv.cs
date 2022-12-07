using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationPanSequenceNumberTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12 };

    public ApplicationPanSequenceNumberTestTlv() : base(_DefaultContentOctets) { }

    public ApplicationPanSequenceNumberTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ApplicationPanSequenceNumber.Tag;
}
