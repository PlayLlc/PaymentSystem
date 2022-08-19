using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageUnpredictableNumberTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x14, 0x2B, 0x24, 0xC1};

    #endregion

    #region Constructor

    public DataStorageUnpredictableNumberTestTlv() : base(_DefaultContentOctets)
    { }

    public DataStorageUnpredictableNumberTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => DataStorageUnpredictableNumber.Tag;
}