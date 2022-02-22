using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Codecs;
using Play.Core;

namespace Play.Ber.Codecs;

public class BerEncodingIdType : EnumObject<PlayEncodingId>
{
    #region Static Metadata

    public static readonly BerEncodingIdType AlphabeticCodec;
    public static readonly BerEncodingIdType AlphaNumericCodec;
    public static readonly BerEncodingIdType AlphabeticNumericSpecialCodec;
    public static readonly BerEncodingIdType CompressedNumericCodec;
    public static readonly BerEncodingIdType NumericCodec;
    public static readonly BerEncodingIdType UnsignedBinaryCodec;
    public static readonly BerEncodingIdType VariableCodec;

    #endregion

    #region Constructor

    static BerEncodingIdType()
    {
        AlphabeticCodec = AlphabeticCodec.EncodingId;
        AlphaNumericCodec = AlphaNumericCodec.EncodingId;
        AlphabeticNumericSpecialCodec = AlphabeticNumericSpecialCodec.EncodingId;
        CompressedNumericCodec = CompressedNumericCodec.EncodingId;
        NumericCodec = NumericCodec.EncodingId;
        UnsignedBinaryCodec = UnsignedIntegerCodec.EncodingId;
        VariableCodec = HexadecimalCodec.EncodingId;
    }

    #endregion
}