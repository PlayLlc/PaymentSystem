using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MaxTimeForProcessingRelayResistanceApduTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {11, 37};

    #endregion

    #region Constructor

    public MaxTimeForProcessingRelayResistanceApduTestTlv() : base(_DefaultContentOctets)
    { }

    public MaxTimeForProcessingRelayResistanceApduTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => MaxTimeForProcessingRelayResistanceApdu.Tag;
}