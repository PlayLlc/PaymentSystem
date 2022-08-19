using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class UnpredictableNumberNumericTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {13, 22, 77, 31};

    #endregion

    #region Constructor

    public UnpredictableNumberNumericTestTlv() : base(_DefaultContentOctets)
    { }

    public UnpredictableNumberNumericTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => UnpredictableNumberNumeric.Tag;
}