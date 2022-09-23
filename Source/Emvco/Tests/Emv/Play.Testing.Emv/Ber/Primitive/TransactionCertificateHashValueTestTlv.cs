using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TransactionCertificateHashValueTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { };

    public TransactionCertificateHashValueTestTlv() : base(_DefaultContentOctets) { }

    public TransactionCertificateHashValueTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TransactionCertificateHashValue.Tag;
}
