using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ReaderContactlessTransactionLimitWhenCvmIsNotOnDeviceTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {12, 24, 48, 19, 07, 20};

    #endregion

    #region Constructor

    public ReaderContactlessTransactionLimitWhenCvmIsNotOnDeviceTestTlv() : base(_DefaultContentOctets)
    { }

    public ReaderContactlessTransactionLimitWhenCvmIsNotOnDeviceTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Tag;
}