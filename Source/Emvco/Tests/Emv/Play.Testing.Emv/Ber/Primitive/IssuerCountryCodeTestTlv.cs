using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerCountryCodeTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {8, 40};

    #endregion

    #region Constructor

    public IssuerCountryCodeTestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerCountryCodeTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerCountryCode.Tag;
}