using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class PositionOfCardVerificationCode3Track1TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x9F, 0x36, 0x27, 0x09, 0x13, 0x6e };

    public PositionOfCardVerificationCode3Track1TestTlv() : base(_DefaultContentOctets) { }

    public PositionOfCardVerificationCode3Track1TestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => PositionOfCardVerificationCode3Track1.Tag;
}
