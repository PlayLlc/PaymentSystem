using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TransactionReferenceCurrencyCodeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 8, 40 };

    public TransactionReferenceCurrencyCodeTestTlv() : base(_DefaultContentOctets) { }

    public TransactionReferenceCurrencyCodeTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TransactionReferenceCurrencyCode.Tag;
}
