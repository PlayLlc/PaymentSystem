using Play.Ber.Identifiers;
using Play.Emv.Templates.FileControlInformation;
using Play.Emv.TestData.Ber.Primitive;

namespace Play.Emv.TestData.Ber.Constructed;

public class FileControlInformationPpseTestTlv : ConstructedTlv
{
    #region Static Metadata

    private static readonly TestTlv[] _DefaultChildren = new TestTlv[] {new DedicatedFileNameTestTlv(), new FileControlInformationProprietaryPpseTestTlv()};

    private static readonly Tag[] _ChildIndex = FileControlInformationPpse.ChildTags;

    #endregion

    #region Constructor

    public FileControlInformationPpseTestTlv() : base(_ChildIndex, _DefaultChildren)
    { }

    public FileControlInformationPpseTestTlv(TestTlv[] children) : base(_ChildIndex, children)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag()
    {
        return FileControlInformationTemplate.Tag;
    }

    #endregion
}