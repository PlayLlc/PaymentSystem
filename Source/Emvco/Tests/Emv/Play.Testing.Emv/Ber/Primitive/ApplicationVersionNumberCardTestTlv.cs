using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationVersionNumberCardTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {21, 12};

    #endregion

    #region Constructor

    public ApplicationVersionNumberCardTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationVersionNumberCardTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ApplicationVersionNumberCard.Tag;
}