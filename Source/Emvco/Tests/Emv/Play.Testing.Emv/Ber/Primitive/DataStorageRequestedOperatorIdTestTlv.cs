using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageRequestedOperatorIdTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {32, 33, 63, 112, 98, 73, 22, 108};

    #endregion

    #region Constructor

    public DataStorageRequestedOperatorIdTestTlv() : base(_DefaultContentOctets)
    { }

    public DataStorageRequestedOperatorIdTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => DataStorageRequestedOperatorId.Tag;
}