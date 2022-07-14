using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class Track1DiscretionaryDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets =
    {
        61, 84, 98, 62, 33, 44, 92, 42, 80, 64,
        62, 83, 99, 61, 34, 44, 92, 43, 80, 65,
        63, 82, 100, 60, 35, 44, 92, 44, 80, 66,
    };

    public Track1DiscretionaryDataTestTlv() : base(_DefaultContentOctets) { }

    public Track1DiscretionaryDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => Track1DiscretionaryData.Tag;
}
