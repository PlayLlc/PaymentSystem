using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerScriptTemplate1TestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        40, 81, 38, 40, 81, 38, 40, 81, 38, 40,
        81, 38, 40, 81, 38, 40, 81, 38, 40, 81,
        38, 40, 81, 38
    };

    #endregion

    #region Constructor

    public IssuerScriptTemplate1TestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerScriptTemplate1TestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerScriptTemplate1.Tag;
}