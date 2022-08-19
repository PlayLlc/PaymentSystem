using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageInputTerminalTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x32, 0x12, 0x08, 0x3c, 0x23, 0x9a, 0x18, 0x20};

    #endregion

    #region Constructor

    public DataStorageInputTerminalTestTlv() : base(_DefaultContentOctets)
    { }

    public DataStorageInputTerminalTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => DataStorageInputTerminal.Tag;
}