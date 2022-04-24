using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class DirectoryEntryTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10,
        0x10, 0x87, 0x01, 0x01, 0x9F, 0x2A, 0x01, 0x03, 0x42, 0x03,
        0x40, 0x81, 0x38, 0x5F, 0x55, 0x02, 0x5F, 0x55, 0x02, 0x55,
        0x53
    };

    private static readonly TagLengthValue[] _DefaultChildren = new TagLengthValue[]
    {
        new(ApplicationIdentifier.Tag, new byte[] {0xA0, 0x00, 0x00, 0x00, 0x03, 0x10, 0x10}.AsSpan()),
        new(ApplicationPriorityIndicator.Tag, new byte[] {0x87, 0x01, 0x01}), new(KernelIdentifier.Tag, new byte[] {0x03}),
        new(IssuerIdentificationNumber.Tag, new byte[] {0x40, 0x81, 0x38}), new(IssuerCountryCodeAlpha2.Tag, new byte[] {0x55, 0x53})
    };

    private static readonly Tag[] _ChildIndex = DirectoryEntry.ChildTags;

    #endregion

    #region Constructor

    public DirectoryEntryTestTlv() : base()
    { }

    public DirectoryEntryTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static TagLengthValue[] GetChildren() => _DefaultChildren;
    public static byte[] GetRawTagLengthValue() => _DefaultContentOctets;
    public override Tag GetTag() => DirectoryEntry.Tag;

    #endregion
}