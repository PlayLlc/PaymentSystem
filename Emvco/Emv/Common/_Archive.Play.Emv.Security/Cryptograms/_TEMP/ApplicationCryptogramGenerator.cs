using System;

using ___TEMP.Play.Emv.Security.Cryptograms;

using Play.Emv.Ber.DataElements;
using Play.Icc.SecureMessaging;

namespace Play.Emv.Security.Cryptograms._TEMP;

internal class ApplicationCryptogramGenerator
{
    #region Instance Values

    private readonly IAesCodec _AesCodec;
    private readonly ITripleDesCodec _TripleDesCodec;

    #endregion

    #region Constructor

    public ApplicationCryptogramGenerator(IAesCodec aesCodec, ITripleDesCodec tripleDesCodec)
    {
        _AesCodec = aesCodec;
        _TripleDesCodec = tripleDesCodec;
    }

    #endregion

    #region Instance Members

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

    #endregion
}