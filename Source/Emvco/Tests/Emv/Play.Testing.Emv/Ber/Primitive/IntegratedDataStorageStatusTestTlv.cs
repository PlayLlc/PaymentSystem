using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IntegratedDataStorageStatusTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x13};

    #endregion

    #region Constructor

    public IntegratedDataStorageStatusTestTlv() : base(_DefaultContentOctets)
    { }

    public IntegratedDataStorageStatusTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => IntegratedDataStorageStatus.Tag;
}