using Play.Codecs;
using Play.Core;

namespace Play.Emv.Ber;

public record BerEncodingIdType : EnumObject<PlayEncodingId>
{
    #region Static Metadata

    private static readonly Dictionary<PlayEncodingId, BerEncodingIdType> _ValueObjectMap;
    public static readonly BerEncodingIdType Empty = new();
    public static readonly BerEncodingIdType AlphabeticCodec;
    public static readonly BerEncodingIdType AlphaNumericCodec;
    public static readonly BerEncodingIdType AlphaNumericSpecialCodec;
    public static readonly BerEncodingIdType CompressedNumericCodec;
    public static readonly BerEncodingIdType NumericCodec;
    public static readonly BerEncodingIdType UnsignedBinaryCodec;
    public static readonly BerEncodingIdType VariableCodec;

    #endregion

    #region Constructor

    public BerEncodingIdType()
    { }

    static BerEncodingIdType()
    {
        AlphabeticCodec = new BerEncodingIdType(Codecs.AlphabeticCodec.EncodingId);
        AlphaNumericCodec = new BerEncodingIdType(Codecs.AlphaNumericCodec.EncodingId);
        AlphaNumericSpecialCodec = new BerEncodingIdType(Codecs.AlphaNumericSpecialCodec.EncodingId);
        CompressedNumericCodec = new BerEncodingIdType(Codecs.CompressedNumericCodec.EncodingId);
        NumericCodec = new BerEncodingIdType(Codecs.NumericCodec.EncodingId);
        UnsignedBinaryCodec = new BerEncodingIdType(UnsignedIntegerCodec.EncodingId);
        VariableCodec = new BerEncodingIdType(HexadecimalCodec.EncodingId);

        _ValueObjectMap = new Dictionary<PlayEncodingId, BerEncodingIdType>
        {
            {AlphabeticCodec, AlphabeticCodec},
            {AlphaNumericCodec, AlphaNumericCodec},
            {AlphaNumericSpecialCodec, AlphaNumericSpecialCodec},
            {CompressedNumericCodec, CompressedNumericCodec},
            {NumericCodec, NumericCodec},
            {UnsignedBinaryCodec, UnsignedBinaryCodec},
            {VariableCodec, VariableCodec}
        };
    }

    public BerEncodingIdType(PlayEncodingId value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingIdType[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(PlayEncodingId value, out EnumObject<PlayEncodingId>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out BerEncodingIdType? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}