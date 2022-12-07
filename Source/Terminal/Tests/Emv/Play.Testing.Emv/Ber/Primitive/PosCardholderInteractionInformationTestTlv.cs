using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class PosCardholderInteractionInformationTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContents = {3, 5, 3};

    #endregion

    #region Constructor

    public PosCardholderInteractionInformationTestTlv() : base(_DefaultContents)
    { }

    public PosCardholderInteractionInformationTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => PosCardholderInteractionInformation.Tag;
}