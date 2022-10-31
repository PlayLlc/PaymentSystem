using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageSlotManagementControlTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x1B };

    public DataStorageSlotManagementControlTestTlv() : base(_DefaultContentOctets) { }

    public DataStorageSlotManagementControlTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataStorageSlotManagementControl.Tag;
}
