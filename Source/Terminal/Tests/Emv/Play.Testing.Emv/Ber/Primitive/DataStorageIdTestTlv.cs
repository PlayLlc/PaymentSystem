using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageIdTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 09, 12, 31, 71, 34, 41, 08, 19, 33 };

    public DataStorageIdTestTlv() : base(_DefaultContentOctets) { }

    public DataStorageIdTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataStorageId.Tag;
}
