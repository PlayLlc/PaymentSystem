using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageApplicationCryptogramTypeTestTlv : TestTlv
{
    private readonly static byte[] _DefaultContentOctets = { 0b0100_0000 };

    public DataStorageApplicationCryptogramTypeTestTlv() : base(_DefaultContentOctets) { }

    public DataStorageApplicationCryptogramTypeTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataStorageApplicationCryptogramType.Tag;
}
