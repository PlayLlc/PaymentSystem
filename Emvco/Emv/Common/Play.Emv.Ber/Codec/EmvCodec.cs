using Play.Ber;
using Play.Ber.Codecs;
using Play.Codecs;

namespace Play.Emv.Ber;

public partial class EmvCodec : BerCodec
{
    #region Static Metadata

    public static readonly BerConfiguration Configuration = new(new Dictionary<PlayEncodingId, PlayCodec>
    {
        {AlphabeticCodec.EncodingId, new AlphabeticCodec()},
        {AlphaNumericCodec.EncodingId, new AlphaNumericCodec()},
        {AlphaNumericSpecialCodec.EncodingId, new AlphaNumericSpecialCodec()},
        {CompressedNumericCodec.EncodingId, new CompressedNumericCodec()},
        {NumericCodec.EncodingId, new NumericCodec()},
        {BinaryCodec.EncodingId, new BinaryCodec()},
        {HexadecimalCodec.EncodingId, new HexadecimalCodec()}
    });

    private static readonly EmvCodec _Codec = new();

    #endregion

    #region Constructor

    public EmvCodec() : base(Configuration)
    { }

    #endregion

    #region Instance Members

    public static EmvCodec GetCodec() => _Codec;

    #endregion
}