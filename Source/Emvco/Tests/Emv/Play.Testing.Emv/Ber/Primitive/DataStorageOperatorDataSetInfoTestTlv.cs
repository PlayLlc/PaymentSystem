using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageOperatorDataSetInfoTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x9B };

    public DataStorageOperatorDataSetInfoTestTlv() : base(_DefaultContentOctets) { }

    public DataStorageOperatorDataSetInfoTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataStorageOperatorDataSetInfo.Tag;
}
