using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class KernelIdentifierTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x03};

    #endregion

    #region Constructor

    public KernelIdentifierTestTlv() : base(_DefaultContentOctets)
    { }

    public KernelIdentifierTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => KernelIdentifier.Tag;

    #endregion
}