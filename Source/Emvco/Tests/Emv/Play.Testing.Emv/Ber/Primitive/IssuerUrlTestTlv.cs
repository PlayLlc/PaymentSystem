using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerUrlTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        (byte) 'h', (byte) 't', (byte) 't', (byte) 'p', (byte) 't', (byte) 'e', (byte) 's', (byte) 't', (byte) 'u', (byte) 'r',
        (byte) 'l'
    };

    #endregion

    #region Constructor

    public IssuerUrlTestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerUrlTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerUrl.Tag;
}