using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class Track2DataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets =
    {
        //PAN
        84, 98, 62, 33, 44, 92,
        //Field Separator
        0xD,
        //Expiry Date
        34, 34, 32, 39,
        //Service Code
        39, 34, 37,
        //Discretionary Data.
        61, 84, 98, 62, 55
    };

    public Track2DataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public Track2DataTestTlv() : base(_DefaultContentOctets)
    {
    }

    public override Tag GetTag() => Track2Data.Tag;
}
