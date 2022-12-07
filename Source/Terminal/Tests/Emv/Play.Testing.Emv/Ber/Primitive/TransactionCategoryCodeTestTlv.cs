using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TransactionCategoryCodeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { (byte) 't' };

    public TransactionCategoryCodeTestTlv() : base(_DefaultContentOctets) { }

    public TransactionCategoryCodeTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TransactionCategoryCode.Tag;
}
