using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TransactionCertificateHashValueTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = {
        32, 14, 56, 64, 33, 27, 18, 30, 114, 90,
        23, 41, 65, 46, 33, 72, 81, 03, 201, 09,
    };

    public TransactionCertificateHashValueTestTlv() : base(_DefaultContentOctets) { }

    public TransactionCertificateHashValueTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TransactionCertificateHashValue.Tag;
}
