using Play.Ber.Identifiers;
using Play.Emv.Templates.FileControlInformation;
using Play.Emv.TestData.Ber.Primitive;

namespace Play.Emv.TestData.Ber.Constructed;

public class FileControlInformationProprietaryPpseTestTlv : ConstructedTlv
{
    #region Static Metadata

    private static readonly TestTlv[] _DefaultChildren = {new FileControlInformationIssuerDiscretionaryPpseTestTlv()};
    private static readonly Tag[] _ChildIndex = FileControlInformationProprietaryPpse.ChildTags;

    #endregion

    #region Constructor

    public FileControlInformationProprietaryPpseTestTlv() : base(_ChildIndex, _DefaultChildren)
    { }

    public FileControlInformationProprietaryPpseTestTlv(TestTlv[] children) : base(_ChildIndex, children)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => 0xA5;

    #endregion
}