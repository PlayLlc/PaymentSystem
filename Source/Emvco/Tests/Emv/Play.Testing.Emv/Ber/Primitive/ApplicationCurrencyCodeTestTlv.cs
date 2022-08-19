using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationCurrencyCodeTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {8, 40};

    #endregion

    #region Constructor

    public ApplicationCurrencyCodeTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationCurrencyCodeTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ApplicationCurrencyCode.Tag;
}