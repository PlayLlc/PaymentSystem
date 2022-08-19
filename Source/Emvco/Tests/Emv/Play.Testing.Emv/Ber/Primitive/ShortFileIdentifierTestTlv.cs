using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ShortFileIdentifierTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {14};

    #endregion

    #region Constructor

    public ShortFileIdentifierTestTlv() : base(_DefaultContentOctets)
    { }

    public ShortFileIdentifierTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ShortFileIdentifier.Tag;
}