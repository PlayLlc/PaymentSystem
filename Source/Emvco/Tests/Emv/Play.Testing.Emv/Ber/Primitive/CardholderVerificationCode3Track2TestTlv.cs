using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class CardholderVerificationCode3Track2TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 24, 34 };

    public CardholderVerificationCode3Track2TestTlv() : base(_DefaultContentOctets) { }

    public CardholderVerificationCode3Track2TestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => CardholderVerificationCode3Track2.Tag;
}
