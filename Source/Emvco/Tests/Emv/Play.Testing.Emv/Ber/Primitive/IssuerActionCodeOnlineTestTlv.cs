using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerActionCodeOnlineTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {144, 14, 181, 51, 91};

    #endregion

    #region Constructor

    public IssuerActionCodeOnlineTestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerActionCodeOnlineTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerActionCodeOnline.Tag;
}