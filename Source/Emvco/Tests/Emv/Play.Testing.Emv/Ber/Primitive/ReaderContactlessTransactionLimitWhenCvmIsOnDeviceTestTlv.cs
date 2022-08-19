using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ReaderContactlessTransactionLimitWhenCvmIsOnDeviceTestTlv : TestTlv
{
    private readonly static byte[] _DefaultContentOctets = { 12, 24, 48, 19, 07, 20 };

    public ReaderContactlessTransactionLimitWhenCvmIsOnDeviceTestTlv() : base(_DefaultContentOctets) { }

    public ReaderContactlessTransactionLimitWhenCvmIsOnDeviceTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ReaderContactlessTransactionLimitWhenCvmIsOnDevice.Tag;
}
