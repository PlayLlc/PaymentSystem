using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ReaderContactlessTransactionLimitWhenCvmIsNotOnDeviceTestTlv : TestTlv
{
    private readonly static byte[] _DefaultContentOctets = { 12, 24, 48, 19, 07, 20 };

    public ReaderContactlessTransactionLimitWhenCvmIsNotOnDeviceTestTlv() : base(_DefaultContentOctets) { }

    public ReaderContactlessTransactionLimitWhenCvmIsNotOnDeviceTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice.Tag;
}
