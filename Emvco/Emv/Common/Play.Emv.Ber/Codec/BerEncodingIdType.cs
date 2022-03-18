using Play.Codecs;
using Play.Core;

namespace Play.Emv.Ber;

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
        AlphabeticCodec = new BerEncodingIdType(Codecs.AlphabeticCodec.EncodingId);
        AlphaNumericCodec = new BerEncodingIdType(Codecs.AlphaNumericCodec.EncodingId);
        AlphaNumericSpecialCodec = new BerEncodingIdType(Codecs.AlphaNumericSpecialCodec.EncodingId);
        CompressedNumericCodec = new BerEncodingIdType(Codecs.CompressedNumericCodec.EncodingId);
        NumericCodec = new BerEncodingIdType(Codecs.NumericCodec.EncodingId);
        UnsignedBinaryCodec = new BerEncodingIdType(UnsignedIntegerCodec.EncodingId);
        VariableCodec = new BerEncodingIdType(HexadecimalCodec.EncodingId);
    }

    public BerEncodingIdType(PlayEncodingId value) : base(value)
    { }

    #endregion
}