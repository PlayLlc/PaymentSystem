using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerCountryCodeAlpha2TestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {(byte) 'R', (byte) 'O'};

    #endregion

    #region Constructor

    public IssuerCountryCodeAlpha2TestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerCountryCodeAlpha2TestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerCountryCodeAlpha2.Tag;
}