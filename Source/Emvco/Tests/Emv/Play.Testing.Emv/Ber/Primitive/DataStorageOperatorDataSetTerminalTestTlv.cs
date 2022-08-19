using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageOperatorDataSetTerminalTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x22, 0x34, 0x7A, 0x9B, 0x19, 0x34, 0xC9, 0x3A, 0x13, 0x33,
    0x22, 0x34, 0x7A, 0x9B, 0x19, 0x34, 0xC9, 0x3A, 0x13, 0x33 };

    public DataStorageOperatorDataSetTerminalTestTlv() : base(_DefaultContentOctets) { }

    public DataStorageOperatorDataSetTerminalTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataStorageOperatorDataSetTerminal.Tag;
}
