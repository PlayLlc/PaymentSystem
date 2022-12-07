using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class BalanceReadAfterGenAcTestTlv : TestTlv
{
    #region Static Metadata

    //Exact length : 6
    private static readonly byte[] _DefaultContent = new byte[] {36, 44, 34, 9, 12, 4};

    #endregion

    #region Constructor

    public BalanceReadAfterGenAcTestTlv() : base(_DefaultContent)
    { }

    public BalanceReadAfterGenAcTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => BalanceReadAfterGenAc.Tag;
}