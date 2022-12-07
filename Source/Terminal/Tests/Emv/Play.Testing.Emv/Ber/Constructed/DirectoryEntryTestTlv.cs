using Play.Ber.Tags;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class DirectoryEntryTestTlv : ConstructedTlv
{
    #region Static Metadata

    private static readonly byte[] _RawTagLengthValue =
    {
        0x61, 0x10, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10,
        0x10, 0x87, 0x01, 0x01, 0x9F, 0x2A, 0x01, 0x03
    };

    private static readonly byte[] _DefaultContentOctets =
    {
        0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10, 0x10, 0x87,
        0x01, 0x01, 0x9F, 0x2A, 0x01, 0x03
    };

    private static readonly TestTlv[] _DefaultChildren =
    {
        new ApplicationDedicatedFileNameTestTlv(), new ApplicationLabelTestTlv(), new ApplicationPriorityIndicatorTestTlv(), new ExtendedSelectionTestTlv(), new KernelIdentifierTestTlv()
    };

    #endregion

    #region Constructor

    public DirectoryEntryTestTlv() : base(DirectoryEntry.ChildTags, _DefaultChildren)
    { }

    public DirectoryEntryTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members
    public static byte[] GetRawTagLengthValue() => _RawTagLengthValue;
    public override Tag GetTag() => DirectoryEntry.Tag;

    #endregion
}