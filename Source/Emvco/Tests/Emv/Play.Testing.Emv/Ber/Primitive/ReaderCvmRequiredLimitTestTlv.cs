using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ReaderCvmRequiredLimitTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {12, 24, 48, 19, 07, 20};

    #endregion

    #region Constructor

    public ReaderCvmRequiredLimitTestTlv() : base(_DefaultContentOctets)
    { }

    public ReaderCvmRequiredLimitTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ReaderCvmRequiredLimit.Tag;
}