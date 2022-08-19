using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageApplicationCryptogramTypeTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0b0100_0000};

    #endregion

    #region Constructor

    public DataStorageApplicationCryptogramTypeTestTlv() : base(_DefaultContentOctets)
    { }

    public DataStorageApplicationCryptogramTypeTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => DataStorageApplicationCryptogramType.Tag;
}