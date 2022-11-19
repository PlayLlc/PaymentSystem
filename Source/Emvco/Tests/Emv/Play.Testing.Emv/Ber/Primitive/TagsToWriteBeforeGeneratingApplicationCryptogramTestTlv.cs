using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TagsToWriteBeforeGeneratingApplicationCryptogramTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = {
        0x9F, 0x75, // Tag
        2, // Length
        12, 23 //Value
    };

    public TagsToWriteBeforeGeneratingApplicationCryptogramTestTlv() : base(_DefaultContentOctets) { }

    public TagsToWriteBeforeGeneratingApplicationCryptogramTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override int GetValueByteCount() => 2;

    public override Tag GetTag() => TagsToWriteBeforeGeneratingApplicationCryptogram.Tag;
}
