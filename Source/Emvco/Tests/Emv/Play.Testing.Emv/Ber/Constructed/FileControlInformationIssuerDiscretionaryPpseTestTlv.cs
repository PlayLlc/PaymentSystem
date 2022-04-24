using Play.Ber.Identifiers;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class FileControlInformationIssuerDiscretionaryPpseTestTlv : ConstructedTlv
{
    #region Static Metadata

    private static readonly TestTlv[] _DefaultChildren =
    {
        new DirectoryEntryTestTlv(),
        new DirectoryEntryTestTlv(new TestTlv[]
        {
            new ApplicationDedicatedFileNameTestTlv(new byte[] {0xA0, 0x00, 0x00, 0x00, 0x98, 0x08, 0x40}),
            new ApplicationPriorityIndicatorTestTlv(new byte[] {0x02}), new KernelIdentifierTestTlv(new byte[] {0x03})
        })
    };

    private static readonly Tag[] _ChildIndex = FileControlInformationIssuerDiscretionaryDataPpse.ChildTags;

    #endregion

    #region Constructor

    public FileControlInformationIssuerDiscretionaryPpseTestTlv() : base(_ChildIndex, _DefaultChildren)
    { }

    public FileControlInformationIssuerDiscretionaryPpseTestTlv(TestTlv[] children) : base(_ChildIndex, children)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => FileControlInformationIssuerDiscretionaryDataTemplate.Tag;

    #endregion
}