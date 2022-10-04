using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class FileControlInformationProprietaryAdfTestTlv : TestTlv
{
    #region Static Data

    private static readonly byte[] _RawTagLengthValue =
    {
        0x61, 0x10, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10,
        0x10, 0x87, 0x01, 0x01, 0x9F, 0x2A, 0x01, 0x03
    };

    private static readonly byte[] _DefaultContentOctets =
    {
        0xBF, 0x0C, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10, 0x10,
        0x87, 1, 13,
        0x50, 0x04, (byte) 't', (byte) 'e', (byte) 's', (byte) 't',
        0x9F, 0x12, 0x04, (byte) 't', (byte) 'e', (byte) 's', (byte) 't',
        0x9F, 0x11, 1, 22,
        0x5F, 0x2D, 2, (byte)'e', (byte)'n'
    };

    private static readonly Tag Tag = FileControlInformationProprietaryAdf.Tag;

    #endregion

    #region Constructors

    public FileControlInformationProprietaryAdfTestTlv() : base(_DefaultContentOctets) { }

    public FileControlInformationProprietaryAdfTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    #endregion

    #region Instance Members

    public static TagLengthValue AsTagLengthValue() => new(Tag, _DefaultContentOctets);
    public static byte[] GetRawTagLengthValue() => _RawTagLengthValue;

    public override Tag GetTag() => FileControlInformationProprietaryAdf.Tag;

    #endregion
}
