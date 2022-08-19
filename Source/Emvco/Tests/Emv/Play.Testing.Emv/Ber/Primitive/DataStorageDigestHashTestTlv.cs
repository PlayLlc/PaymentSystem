using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageDigestHashTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x09, 0x12, 0x31, 0x7C, 0x34, 0x41, 0x08, 0x19};

    #endregion

    #region Constructor

    public DataStorageDigestHashTestTlv() : base(_DefaultContentOctets)
    { }

    public DataStorageDigestHashTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => DataStorageDigestHash.Tag;
}