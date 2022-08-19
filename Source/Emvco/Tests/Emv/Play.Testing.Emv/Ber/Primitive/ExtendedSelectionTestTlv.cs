using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ExtendedSelectionTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {12, 13};

    #endregion

    #region Constructor

    public ExtendedSelectionTestTlv() : base(_DefaultContentOctets)
    { }

    public ExtendedSelectionTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ExtendedSelection.Tag;
}