using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class FileControlInformationIssuerDiscretionaryDataPpseTestTlv : ConstructedTlv
{
    private static readonly TestTlv[] _DefaultChildren = { new DirectoryEntryTestTlv(), new SelectionDataObjectListTestTlv(), new TerminalCategoriesSupportedListTestTlv() };
    private static readonly Tag[] _ChildIndex = new[] { DirectoryEntry.Tag, SelectionDataObjectList.Tag, TerminalCategoriesSupportedList.Tag };

    public FileControlInformationIssuerDiscretionaryDataPpseTestTlv() : base(_ChildIndex, _DefaultChildren) { }

    public FileControlInformationIssuerDiscretionaryDataPpseTestTlv(Tag[] childRank, params TestTlv[] children) : base(childRank, children)
    {
    }

    public override Tag GetTag() => FileControlInformationIssuerDiscretionaryDataPpse.Tag;
}
