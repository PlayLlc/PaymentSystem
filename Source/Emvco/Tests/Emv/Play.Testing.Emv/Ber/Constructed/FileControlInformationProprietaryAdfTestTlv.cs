using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class FileControlInformationProprietaryAdfTestTlv : ConstructedTlv
{
    #region Static Data


    private static readonly TestTlv[] _DefaultChildren =
    {
        new ApplicationLabelTestTlv(), new ApplicationPreferredNameTestTlv(), new ApplicationPriorityIndicatorTestTlv(), new FileControlInformationIssuerDiscretionaryDataAdfTestTlv(),
        new IssuerCodeTableIndexTestTlv(), new LanguagePreferenceTestTlv(), new ProcessingOptionsDataObjectListTestTlv()
    };

    private static readonly Tag[] _ChildIndex = new[]
    {
        ApplicationLabel.Tag, ApplicationPreferredName.Tag, ApplicationPriorityIndicator.Tag, FileControlInformationIssuerDiscretionaryDataDdf.Tag,
        IssuerCodeTableIndex.Tag, LanguagePreference.Tag, ProcessingOptionsDataObjectList.Tag
    };

    #endregion

    #region Constructors

    public FileControlInformationProprietaryAdfTestTlv() : base(_ChildIndex, _DefaultChildren) { }

    public FileControlInformationProprietaryAdfTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    #endregion

    #region Instance Members

    public override Tag GetTag() => FileControlInformationProprietaryAdf.Tag;

    #endregion
}
