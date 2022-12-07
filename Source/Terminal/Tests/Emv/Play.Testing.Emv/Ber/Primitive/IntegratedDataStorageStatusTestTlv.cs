using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IntegratedDataStorageStatusTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x13 };

    public IntegratedDataStorageStatusTestTlv() : base(_DefaultContentOctets) { }

    public IntegratedDataStorageStatusTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IntegratedDataStorageStatus.Tag;
}
