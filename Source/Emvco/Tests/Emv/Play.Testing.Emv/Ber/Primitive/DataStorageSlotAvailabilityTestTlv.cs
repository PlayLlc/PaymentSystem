using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageSlotAvailabilityTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0xC9 };

    public DataStorageSlotAvailabilityTestTlv() : base(_DefaultContentOctets) { }

    public DataStorageSlotAvailabilityTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataStorageSlotAvailability.Tag;
}
