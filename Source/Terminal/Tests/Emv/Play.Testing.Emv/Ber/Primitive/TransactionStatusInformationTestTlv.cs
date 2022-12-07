using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TransactionStatusInformationTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12, 38 };

    public TransactionStatusInformationTestTlv() : base(_DefaultContentOctets) { }
    
    public TransactionStatusInformationTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TransactionStatusInformation.Tag;
}
