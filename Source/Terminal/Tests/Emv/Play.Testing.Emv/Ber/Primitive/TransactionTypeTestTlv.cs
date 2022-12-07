using Play.Ber.Tags;
using Play.Emv.Ber.DataElements.Transaction;

namespace Play.Testing.Emv.Ber.Primitive;

public class TransactionTypeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12 };

    public TransactionTypeTestTlv() : base(_DefaultContentOctets) { }
    public TransactionTypeTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TransactionType.Tag;
}
