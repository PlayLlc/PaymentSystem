using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageSlotManagementControlTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x1B};

    #endregion

    #region Constructor

    public DataStorageSlotManagementControlTestTlv() : base(_DefaultContentOctets)
    { }

    public DataStorageSlotManagementControlTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => DataStorageSlotManagementControl.Tag;
}