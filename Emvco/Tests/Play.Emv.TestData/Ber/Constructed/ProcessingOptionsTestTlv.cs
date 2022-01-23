using Play.Ber.Identifiers;
using Play.Emv.Templates.ResponseMessages;
using Play.Emv.TestData.Ber.Primitive;

namespace Play.Emv.TestData.Ber.Constructed;

public class ProcessingOptionsTestTlv : ConstructedTlv
{
    #region Static Metadata

    private static readonly TestTlv[] _DefaultChildren = new TestTlv[] {new ApplicationFileLocatorTestTlv(), new ApplicationInterchangeProfileTestTlv()};

    private static readonly Tag[] _ChildIndex = ProcessingOptions.ChildTags;

    #endregion

    #region Constructor

    public ProcessingOptionsTestTlv() : base(_ChildIndex, _DefaultChildren)
    { }

    public ProcessingOptionsTestTlv(TestTlv[] children) : base(_ChildIndex, children)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag()
    {
        return ResponseMessageTemplate.Tag;
    }

    #endregion
}