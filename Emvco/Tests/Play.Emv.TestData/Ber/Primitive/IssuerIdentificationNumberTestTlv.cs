using Play.Ber.Identifiers;
using Play.Emv.DataElements;

namespace Play.Emv.TestData.Ber.Primitive;

public class IssuerIdentificationNumberTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x40, 0x81, 0x38};

    #endregion

    #region Constructor

    public IssuerIdentificationNumberTestTlv() : base(_DefaultContentOctets)
    { }

    public IssuerIdentificationNumberTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => IssuerIdentificationNumber.Tag;

    #endregion
}