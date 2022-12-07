using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class ResponseMessageTemplateFormat2TestTlv : ConstructedTlv
{
    private static readonly TestTlv[] _DefaultChildren =
    {
        new AdditionalTerminalCapabilitiesTestTlv(),
        new ApplicationCryptogramTestTlv(),
        new CvmResultsTestTlv()
    };

    private static readonly Tag[] _ChildIndex = { AdditionalTerminalCapabilities.Tag, ApplicationCryptogram.Tag, CvmResults.Tag };

    public ResponseMessageTemplateFormat2TestTlv() : base(_ChildIndex, _DefaultChildren) { }

    public ResponseMessageTemplateFormat2TestTlv(Tag[] childRank, params TestTlv[] children) : base(childRank, children) {}

    public override Tag GetTag() => ResponseMessageTemplateFormat2.Tag;
}
