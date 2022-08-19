using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerActionCodeDenialTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {23, 14, 144, 51, 91};

    #endregion

    #region Constructor

    public IssuerActionCodeDenialTestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerActionCodeDenialTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerActionCodeDenial.Tag;
}