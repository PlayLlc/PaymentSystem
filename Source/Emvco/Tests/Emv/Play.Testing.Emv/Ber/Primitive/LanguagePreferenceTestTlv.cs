using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class LanguagePreferenceTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { (byte)'R', (byte)'O', (byte)'U', (byte)'S' };

    public LanguagePreferenceTestTlv() : base(_DefaultContentOctets) { }

    public LanguagePreferenceTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => LanguagePreference.Tag;
}
