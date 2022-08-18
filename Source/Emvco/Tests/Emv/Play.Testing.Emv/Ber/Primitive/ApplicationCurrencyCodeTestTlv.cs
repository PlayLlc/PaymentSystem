using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;
public class ApplicationCurrencyCodeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 8, 40 };

    public ApplicationCurrencyCodeTestTlv() : base(_DefaultContentOctets) { }

    public ApplicationCurrencyCodeTestTlv(byte[] contentOctets) : base(contentOctets)
    {

    }

    public override Tag GetTag() => ApplicationCurrencyCode.Tag;
}

