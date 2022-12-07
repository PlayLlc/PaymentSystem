using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TornRecordTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = {
        0x5A, // Tag
        9, // Length
        12, 23, 33, 13, 15, 12, 23, 33, 13, //Value,
        0x5F, 0x34, //Pan Sequence Number
        1,
        12
    };

    public TornRecordTestTlv() : base(_DefaultContentOctets) { }

    public TornRecordTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TornRecord.Tag;
}
