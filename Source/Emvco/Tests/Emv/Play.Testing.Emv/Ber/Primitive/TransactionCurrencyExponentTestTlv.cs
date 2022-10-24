using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TransactionCurrencyExponentTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 7 };

    public TransactionCurrencyExponentTestTlv() : base(_DefaultContentOctets) { }

    public TransactionCurrencyExponentTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TransactionCurrencyExponent.Tag;
}
