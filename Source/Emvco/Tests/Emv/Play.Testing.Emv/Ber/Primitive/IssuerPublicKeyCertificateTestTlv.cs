using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerPublicKeyCertificateTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {40, 81, 38};

    #endregion

    #region Constructor

    public IssuerPublicKeyCertificateTestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerPublicKeyCertificateTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerPublicKeyCertificate.Tag;
}