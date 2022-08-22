using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class Track1DataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets =
    {
        //Format Code.
        (byte) 'B',
        //PAN
        84, 98, 62, 33, 44, 92, 42, 80, 64,
        //Field Separator
        (byte) '^',
        //Name
        62, 83, 99, 61, 34, 44, 92, 43, 80, 64,
        63, 82, 100, 60, 35, 44, 92, 44, 80, 64,
        //Field Separator
        (byte) '^',
        //Expiry Date
        34, 34, 32, 39,
        //Service Code
        39, 34, 37,
        //Discretionary Data.
        61, 84, 98, 62, (byte)'^', 44, 92, 42, 80, 64,
        62, 83, 99, 61, (byte)'^', 44, 92, 43, 80, 65,
    };

    public Track1DataTestTlv() : base(_DefaultContentOctets) { }

    public Track1DataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => Track1Data.Tag;
}
