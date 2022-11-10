using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TagsToReadTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 4 };

    public TagsToReadTestTlv() : base(_DefaultContentOctets) { }

    public TagsToReadTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TagsToRead.Tag;
}
