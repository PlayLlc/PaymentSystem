using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageOperatorDataSetInfoTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x9B};

    #endregion

    #region Constructor

    public DataStorageOperatorDataSetInfoTestTlv() : base(_DefaultContentOctets)
    { }

    public DataStorageOperatorDataSetInfoTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => DataStorageOperatorDataSetInfo.Tag;
}