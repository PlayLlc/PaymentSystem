using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerApplicationDataTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        18, 32, 34, 44, 67, 18, 114, 53, 109, 61,
        28, 15, 209, 20
    };

    #endregion

    #region Constructor

    public IssuerApplicationDataTestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerApplicationDataTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerApplicationData.Tag;
}