using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageSlotAvailabilityTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0xC9};

    #endregion

    #region Constructor

    public DataStorageSlotAvailabilityTestTlv() : base(_DefaultContentOctets)
    { }

    public DataStorageSlotAvailabilityTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => DataStorageSlotAvailability.Tag;
}