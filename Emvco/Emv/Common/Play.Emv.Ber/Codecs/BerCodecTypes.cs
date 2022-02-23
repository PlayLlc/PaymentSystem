using Play.Codecs;
using Play.Core;

namespace Play.Ber.Codecs;

public record BerEncodingIdType : EnumObject<PlayEncodingId>
{
    #region Static Metadata

    public static readonly BerEncodingIdType AlphabeticCodec;
    public static readonly BerEncodingIdType AlphaNumericCodec;
    public static readonly BerEncodingIdType AlphaNumericSpecialCodec;
    public static readonly BerEncodingIdType CompressedNumericCodec;
    public static readonly BerEncodingIdType NumericCodec;
    public static readonly BerEncodingIdType UnsignedBinaryCodec;
    public static readonly BerEncodingIdType VariableCodec;

    #endregion

    #region Constructor

    static BerEncodingIdType()
    {
        AlphabeticCodec = new BerEncodingIdType(Play.Codecs.AlphabeticCodec.EncodingId);
        AlphaNumericCodec = new BerEncodingIdType(Play.Codecs.AlphaNumericCodec.EncodingId);
        AlphaNumericSpecialCodec = new BerEncodingIdType(Play.Codecs.AlphaNumericSpecialCodec.EncodingId);
        CompressedNumericCodec = new BerEncodingIdType(Play.Codecs.CompressedNumericCodec.EncodingId);
        NumericCodec = new BerEncodingIdType(Play.Codecs.NumericCodec.EncodingId);
        UnsignedBinaryCodec = new BerEncodingIdType(UnsignedIntegerCodec.EncodingId);
        VariableCodec = new BerEncodingIdType(HexadecimalCodec.EncodingId);
    }

    public BerEncodingIdType(PlayEncodingId value) : base(value)
    { }

    #endregion
}