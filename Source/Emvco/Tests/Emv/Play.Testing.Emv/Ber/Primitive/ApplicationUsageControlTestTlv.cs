using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationUsageControlTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {21, 12};

    #endregion

    #region Constructor

    public ApplicationUsageControlTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationUsageControlTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ApplicationUsageControl.Tag;
}