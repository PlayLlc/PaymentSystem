using Play.Ber.Tags;
using Play.Emv.Ber.Templates;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class FileControlInformationDdfTestTlv : ConstructedTlv
{
    private static readonly TestTlv[] _DefaultChildren = { new DedicatedFileNameTestTlv(), new FileControlInformationProprietaryDdfTestTlv() };
    private static readonly Tag[] _ChildIndex = new[] { DedicatedFileName.Tag, FileControlInformationProprietaryDdf.Tag };

    public FileControlInformationDdfTestTlv() : base(_ChildIndex, _DefaultChildren) { }

    public FileControlInformationDdfTestTlv(Tag[] childRank, params TestTlv[] children) : base(childRank, children)
    { }

    public override Tag GetTag() => FileControlInformationDdf.Tag;
}
