using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MinTimeForProcessingRelayResistanceApduTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {9, 64};

    #endregion

    #region Constructor

    public MinTimeForProcessingRelayResistanceApduTestTlv() : base(_DefaultContentOctets)
    { }

    public MinTimeForProcessingRelayResistanceApduTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => MinTimeForProcessingRelayResistanceApdu.Tag;
}