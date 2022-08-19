using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageOperatorDataSetInfoForReaderTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x3B};

    #endregion

    #region Constructor

    public DataStorageOperatorDataSetInfoForReaderTestTlv() : base(_DefaultContentOctets)
    { }

    public DataStorageOperatorDataSetInfoForReaderTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => DataStorageOperatorDataSetInfoForReader.Tag;
}