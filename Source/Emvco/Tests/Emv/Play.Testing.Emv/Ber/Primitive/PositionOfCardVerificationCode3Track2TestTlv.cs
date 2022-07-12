using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class PositionOfCardVerificationCode3Track2TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x9F, 0x36 };

    public PositionOfCardVerificationCode3Track2TestTlv() : base(_DefaultContentOctets) { }

    public PositionOfCardVerificationCode3Track2TestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => PositionOfCardVerificationCode3Track2.Tag;
}
