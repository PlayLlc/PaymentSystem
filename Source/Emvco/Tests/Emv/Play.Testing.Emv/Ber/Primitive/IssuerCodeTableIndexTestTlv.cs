using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerCodeTableIndexTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {47};

    #endregion

    #region Constructor

    public IssuerCodeTableIndexTestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerCodeTableIndexTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerCodeTableIndex.Tag;
}