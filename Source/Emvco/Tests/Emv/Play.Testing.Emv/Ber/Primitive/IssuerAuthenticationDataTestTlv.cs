using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerAuthenticationDataTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {123, 222, 15, 16, 39, 190, 34, 212, 138, 47};

    #endregion

    #region Constructor

    public IssuerAuthenticationDataTestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerAuthenticationDataTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerAuthenticationData.Tag;
}