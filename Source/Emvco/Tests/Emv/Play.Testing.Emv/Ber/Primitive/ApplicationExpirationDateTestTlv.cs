using Play.Ber.InternalFactories;
using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationExpirationDateTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {21, 12, 22};

    #endregion

    #region Constructor

    public ApplicationExpirationDateTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationExpirationDateTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Serialization

    public new byte[] EncodeTagLengthValue()
    {
        TagLength tl = new(GetTag(), _ContentOctets);
        Span<byte> result = new byte[tl.GetTagLengthValueByteCount()];

        tl.Encode().CopyTo(result);
        _ContentOctets.CopyTo(result[tl.GetValueOffset()..]);

        return result.ToArray();
    }

    #endregion

    #region Instance Members

    public override Tag GetTag() => ApplicationExpirationDate.Tag;

    #endregion
}