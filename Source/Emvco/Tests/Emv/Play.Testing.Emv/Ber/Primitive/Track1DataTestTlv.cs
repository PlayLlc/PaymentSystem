using Play.Ber.Identifiers;

namespace Play.Testing.Emv.Ber.Primitive;

public class Track1DataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets =
    {
        61, 34, 98, 12, 33, 44, 22, 12, 80, 64,
        62, 33, 99, 11, 33, 44, 22, 12, 80, 64,
        63, 32, 100, 10, 33, 44, 22, 12, 80, 64,
        64, 31, 101, 9, 33, 44, 22, 12, 80, 64,
        65, 30, 102, 8, 33, 44, 22, 12, 80, 64,
        66, 29, 103, 7, 33, 44, 22, 12, 80, 64,
        67, 28, 104, 6, 33, 44, 22, 12, 80, 64,
        68, 27, 105, 5, 33, 44
    };

    public Track1DataTestTlv() : base(_DefaultContentOctets) { }

    public Track1DataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => throw new NotImplementedException();
}
