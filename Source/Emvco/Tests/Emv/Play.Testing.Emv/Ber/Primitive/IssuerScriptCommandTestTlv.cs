using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerScriptCommandTestTlv : TestTlv
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

    public IssuerScriptCommandTestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerScriptCommandTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IssuerScriptCommand.Tag;
}