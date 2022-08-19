using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationIdentifierTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01, 0x12};

    #endregion

    #region Constructor

    public ApplicationIdentifierTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationIdentifierTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ApplicationIdentifier.Tag;
}