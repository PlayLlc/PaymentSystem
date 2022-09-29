using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TransactionReferenceCurrencyExponentTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 7 };

    public TransactionReferenceCurrencyExponentTestTlv() : base(_DefaultContentOctets) { }

    public TransactionReferenceCurrencyExponentTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TransactionReferenceCurrencyExponent.Tag;
}
