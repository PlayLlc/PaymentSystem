using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class DirectoryEntryTestTlv : TestTlv
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

    private static readonly Tag[] _ChildIndex = DirectoryEntry.ChildTags;
    private static readonly Tag Tag = DirectoryEntry.Tag;

    #endregion

    #region Constructor

    public DirectoryEntryTestTlv() : base(_DefaultContentOctets)
    { }

    public DirectoryEntryTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static TagLengthValue AsTagLengthValue() => new(Tag, _DefaultContentOctets);
    public static byte[] GetRawTagLengthValue() => _RawTagLengthValue;
    public override Tag GetTag() => DirectoryEntry.Tag;

    #endregion
}