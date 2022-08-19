using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageSummaryStatusTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x7E};

    #endregion

    #region Constructor

    public DataStorageSummaryStatusTestTlv() : base(_DefaultContentOctets)
    { }

    public DataStorageSummaryStatusTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => DataStorageSummaryStatus.Tag;
}