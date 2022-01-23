using System;

using Play.Icc.SecureMessaging;

namespace Play.Emv.Security.Cryptograms._TEMP
{
    internal class ApplicationCryptogramGenerator
    {
        private readonly IAesCodec _AesCodec;
        private readonly ITripleDesCodec _TripleDesCodec;

        public ApplicationCryptogramGenerator(IAesCodec aesCodec, ITripleDesCodec tripleDesCodec)
        {
            _AesCodec = aesCodec;
            _TripleDesCodec = tripleDesCodec;
        }

        public byte[] Create(ReadOnlySpan<byte> input, ApplicationCryptogram version)
        { }

        public byte[] Create(ReadOnlySpan<byte> input, CryptographicChecksum version)
        {
            if (version == ApplicationCryptogramVersion._5)
                return CreateVersion5Cryptogram(input);

            return CreateVersion6Cryptogram(input);
        }

        private byte[] CreateVersion5Cryptogram(ReadOnlySpan<byte> input)
        { }

        // AES
        private byte[] CreateVersion6Cryptogram(ReadOnlySpan<byte> input)
        { }
    }
}