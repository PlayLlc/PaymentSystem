using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageVersionNumberTerminalTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        0x13, 0x25, 0xC9, 0x25, 0xE3, 0x18, 0x22, 0x99, 0x10, 0x13,
        0x25, 0xC9, 0x25, 0xE3, 0x18, 0x22, 0x99, 0x10
    };

    #endregion

    #region Constructor

    public DataStorageVersionNumberTerminalTestTlv() : base(_DefaultContentOctets)
    { }

    public DataStorageVersionNumberTerminalTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => DataStorageVersionNumberTerminal.Tag;
}