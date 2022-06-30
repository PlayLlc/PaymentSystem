using System.Collections.Generic;

using Play.Codecs;

namespace Play.Ber.Tests.BerCodec
{
    internal static class BerCodecTestDataConfiguration
    {
        public static readonly BerConfiguration EmvBerCodecConfiguration = new(new Dictionary<PlayEncodingId, PlayCodec>
        {
            {AlphabeticCodec.EncodingId, new AlphabeticCodec()},
            {AlphaNumericCodec.EncodingId, new AlphaNumericCodec()},
            {AlphaNumericSpecialCodec.EncodingId, new AlphaNumericSpecialCodec()},
            {CompressedNumericCodec.EncodingId, new CompressedNumericCodec()},
            {NumericCodec.EncodingId, new NumericCodec()},
            {BinaryCodec.EncodingId, new BinaryCodec()},
            {HexadecimalCodec.EncodingId, new HexadecimalCodec()}
        });
    }
}
