using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;
using Play.Randoms;

namespace Play.Testing.Emv.Ber.Primitive;

public class PaymentAccountReferenceTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContent = Enumerable.Range(0, 29).Select(_ => (byte)Randomize.AlphaNumeric.Char()).ToArray();

    public PaymentAccountReferenceTestTlv() : base(_DefaultContent) { }

    public PaymentAccountReferenceTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => PaymentAccountReference.Tag;
}
