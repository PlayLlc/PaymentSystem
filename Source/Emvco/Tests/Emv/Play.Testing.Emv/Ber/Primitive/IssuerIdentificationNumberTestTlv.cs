using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerIdentificationNumberTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = { 40, 81, 38 };

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