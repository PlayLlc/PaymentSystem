using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class PhoneMessageTableTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = new byte[] { };

    public PhoneMessageTableTestTlv() : base(_DefaultContentOctets) { }

    public PhoneMessageTableTestTlv(byte[] contentOctets) : base(contentOctets) {}

    public override Tag GetTag() => PhoneMessageTable.Tag;
}
