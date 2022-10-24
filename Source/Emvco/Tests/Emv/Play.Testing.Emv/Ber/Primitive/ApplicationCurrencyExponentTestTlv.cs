using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationCurrencyExponentTestTlv : TestTlv
{
    private readonly static byte[] _DefaultContentOctets = { 4 };

    public ApplicationCurrencyExponentTestTlv() : base(_DefaultContentOctets) { }

    public ApplicationCurrencyExponentTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    public override Tag GetTag() => ApplicationCurrencyExponent.Tag;
}
