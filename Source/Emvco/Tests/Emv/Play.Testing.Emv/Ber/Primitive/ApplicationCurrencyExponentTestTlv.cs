using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationCurrencyExponentTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {4};

    #endregion

    #region Constructor

    public ApplicationCurrencyExponentTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationCurrencyExponentTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ApplicationCurrencyExponent.Tag;
}