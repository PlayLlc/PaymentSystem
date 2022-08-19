using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerActionCodeDefaultTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {32, 34, 45, 16, 144};

    #endregion

    #region Constructor

    public IssuerActionCodeDefaultTestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerActionCodeDefaultTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerActionCodeDefault.Tag;
}