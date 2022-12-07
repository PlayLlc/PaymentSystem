using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TransactionCurrencyCodeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 8, 40 };

    public TransactionCurrencyCodeTestTlv() : base(_DefaultContentOctets) { }

    public TransactionCurrencyCodeTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TransactionCurrencyCode.Tag;
}
