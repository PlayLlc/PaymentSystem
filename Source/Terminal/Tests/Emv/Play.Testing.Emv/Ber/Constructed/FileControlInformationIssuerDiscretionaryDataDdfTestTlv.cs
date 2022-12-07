using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class FileControlInformationIssuerDiscretionaryDataDdfTestTlv : ConstructedTlv
{
    private static readonly TestTlv[] _DefaultChildren = { new ApplicationCapabilitiesInformationTestTlv() };
    private static readonly Tag[] _ChildIndex = new[] { ApplicationCapabilitiesInformation.Tag };

    public FileControlInformationIssuerDiscretionaryDataDdfTestTlv() : base(_ChildIndex, _DefaultChildren) { }

    public FileControlInformationIssuerDiscretionaryDataDdfTestTlv(Tag[] childRank, params TestTlv[] children) : base(childRank, children)
    {
    }

    public override Tag GetTag() => FileControlInformationIssuerDiscretionaryDataDdf.Tag;
}
