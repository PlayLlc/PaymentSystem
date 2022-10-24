using Play.Ber.Tags;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

namespace Play.Testing.Emv.Ber.Constructed;

public class FileControlInformationProprietaryAdfTestTlv : TestTlv
{
    #region Static Data

    private static readonly byte[] _DefaultContentOctets =
    {
        0x50, 0x04, (byte) 't', (byte) 'e', (byte) 's', (byte) 't',
        0x9F, 0x12, 0x04, (byte) 't', (byte) 'e', (byte) 's', (byte) 't',
        0x87, 1, 13,
        0xBF, 0x0C, 0x00,
        0x9F, 0x11, 1, 22,
        0x5F, 0x2D, 2, (byte)'e', (byte)'n',
        0x9F, 0x38, 0x00
    };

    #endregion

    #region Constructors

    public FileControlInformationProprietaryAdfTestTlv() : base(_DefaultContentOctets) { }

    public FileControlInformationProprietaryAdfTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    #endregion

    #region Instance Members

    public override Tag GetTag() => FileControlInformationProprietaryAdf.Tag;

    #endregion
}
