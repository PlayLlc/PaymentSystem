using Play.Ber.Tags;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class FileControlInformationIssuerDiscretionaryDataDdfTestTlv : ConstructedTlv
{
    public FileControlInformationIssuerDiscretionaryDataDdfTestTlv(Tag[] childRank, params TestTlv[] children) : base(childRank, children)
    {
    }

    public override Tag GetTag() => FileControlInformationIssuerDiscretionaryDataDdf.Tag;
}
