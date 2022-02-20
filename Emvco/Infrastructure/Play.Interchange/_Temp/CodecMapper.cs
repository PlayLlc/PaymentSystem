using System.Text;

using Play.Codecs;
using Play.Codecs.Integers;
using Play.Codecs.Strings;

namespace Play.Interchange.Codecs;

internal class CodecMapper
{
    #region Instance Values

    private readonly Dictionary<string, Encoding> _CodecMap = new()
    {
        {nameof(Alphabetic), PlayEncoding.Alphabetic},
        {nameof(AlphaNumeric), PlayEncoding.AlphaNumeric},
        {nameof(AlphaNumericSpecial), PlayEncoding.AlphaNumericSpecial},
        {nameof(AlphaSpecial), PlayEncoding.AlphaSpecial},

        // TODO: Needs to be UnsignedBinaryCodec. Move logic from Play.Emv.Codecs to Play.Codecs
        {nameof(Binary), PlayEncoding.Binary},
        {nameof(CompressedNumeric), PlayEncoding.CompressedNumeric},
        {nameof(Numeric), PlayEncoding.Numeric},
        {nameof(NumericSpecial), PlayEncoding.NumericSpecial},
        {nameof(SignedNumeric), PlayEncoding.SignedNumeric}
    };

    #endregion
}