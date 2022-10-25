using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class FileControlInformationIssuerDiscretionaryDataAdfTestTlv : ConstructedTlv
{

    private static readonly TestTlv[] _DefaultChildren = { new LogEntryTestTlv(), new ApplicationCapabilitiesInformationTestTlv() };
    private static readonly Tag[] _ChildIndex = new[] { LogEntry.Tag, ApplicationCapabilitiesInformation.Tag };

    public FileControlInformationIssuerDiscretionaryDataAdfTestTlv() : base(_ChildIndex, _DefaultChildren) { }

    public FileControlInformationIssuerDiscretionaryDataAdfTestTlv(Tag[] childRank, params TestTlv[] children) : base(childRank, children)
    {
    }

    public override Tag GetTag() => FileControlInformationIssuerDiscretionaryDataDdf.Tag;
}
