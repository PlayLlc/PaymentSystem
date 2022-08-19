using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerScriptTemplate2TestTlv : TestTlv
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

    public IssuerScriptTemplate2TestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerScriptTemplate2TestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerScriptTemplate2.Tag;
}