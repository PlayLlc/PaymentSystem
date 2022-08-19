using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive.Acquirer;

public class AcquirerIdentifierTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {6, 13, 22, 8, 11, 22};

    #endregion

    #region Constructor

    public AcquirerIdentifierTestTlv() : base(_DefaultContentOctets)
    { }

    public AcquirerIdentifierTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => AcquirerIdentifier.Tag;
}