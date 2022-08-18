using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageRequestedOperatorIdTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 32, 33, 63, 112, 98, 73, 22, 108 };

    public DataStorageRequestedOperatorIdTestTlv() : base(_DefaultContentOctets) { }

    public DataStorageRequestedOperatorIdTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataStorageRequestedOperatorId.Tag;
}
