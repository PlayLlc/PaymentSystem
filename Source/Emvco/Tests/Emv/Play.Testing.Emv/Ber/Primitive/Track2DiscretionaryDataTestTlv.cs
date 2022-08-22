using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class Track2DiscretionaryDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets =
    {
        1, 2, 3, 4, 5, 6, 7, 8, 9,
    };

    public Track2DiscretionaryDataTestTlv() : base(_DefaultContentOctets) { }

    public Track2DiscretionaryDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => Track2DiscretionaryData.Tag;
}
