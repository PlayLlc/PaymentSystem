using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TagsToWriteAfterGeneratingApplicationCryptogramTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = {
        0x9F, 0x75, // Tag
        2, // Length
        12, 23 //Value
    };

    public TagsToWriteAfterGeneratingApplicationCryptogramTestTlv() : base(_DefaultContentOctets) { }

    public TagsToWriteAfterGeneratingApplicationCryptogramTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override byte[] EncodeTagLengthValue() => _DefaultContentOctets;

    public override int GetValueByteCount() => 2;

    public override Tag GetTag() => TagsToWriteAfterGeneratingApplicationCryptogram.Tag;
}
