using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class FileControlInformationProprietaryDdfTestTlv : ConstructedTlv
{
    private static readonly TestTlv[] _DefaultChildren = { new FileControlInformationIssuerDiscretionaryDataDdfTestTlv(),
    new ShortFileIdentifierTestTlv() };
    private static readonly Tag[] _ChildIndex = new[] { FileControlInformationIssuerDiscretionaryDataDdf.Tag,
    ShortFileIdentifier.Tag };

    public FileControlInformationProprietaryDdfTestTlv() : base(_ChildIndex, _DefaultChildren) { }
    public FileControlInformationProprietaryDdfTestTlv(Tag[] childRank, params TestTlv[] children) : base(childRank, children)
    {
    }

    public override Tag GetTag() => FileControlInformationProprietaryDdf.Tag;
}
