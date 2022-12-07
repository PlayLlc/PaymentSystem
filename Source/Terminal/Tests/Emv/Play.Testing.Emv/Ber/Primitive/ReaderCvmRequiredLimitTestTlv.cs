using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ReaderCvmRequiredLimitTestTlv : TestTlv
{
    private readonly static byte[] _DefaultContentOctets = { 12, 24, 48, 19, 07, 20 };

    public ReaderCvmRequiredLimitTestTlv() : base(_DefaultContentOctets) { }

    public ReaderCvmRequiredLimitTestTlv(byte[] contentOctets) : base(contentOctets) { }

    public override Tag GetTag() => ReaderCvmRequiredLimit.Tag;
}
