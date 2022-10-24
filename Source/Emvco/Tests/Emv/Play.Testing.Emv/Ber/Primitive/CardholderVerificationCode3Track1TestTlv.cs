using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class CardholderVerificationCode3Track1TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 24, 34 };

    public CardholderVerificationCode3Track1TestTlv() : base(_DefaultContentOctets) { }

    public CardholderVerificationCode3Track1TestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => CardholderVerificationCode3Track1.Tag;
}
