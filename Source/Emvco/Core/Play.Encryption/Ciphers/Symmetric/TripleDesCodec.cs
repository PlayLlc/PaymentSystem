using System.Security.Cryptography;

using Microsoft.Toolkit.HighPerformance.Buffers;

namespace Play.Encryption.Ciphers.Symmetric;

/// <summary>
///     An 8 block cypher that applies the DES algorithm three times to each block
///     The block size is the basic unit of data that can be encrypted or decrypted in one operation.
///     Messages longer than the block size are handled as successive blocks;
///     messages shorter than the block size must be padded with extra bits to reach the size of a block. Valid block sizes are determined by the symmetric algorithm used.
/// </summary>
public class TripleDesCodec : IBlockCipher
{
    #region Instance Values

    private readonly BlockSize _BlockSize;
    private readonly BlockCipherMode _CipherMode;
    private readonly KeySize _KeySize;
    private readonly BlockPaddingMode _PaddingMode;
    private readonly IPreprocessPlainText _Preprocessor;

    #endregion

    #region Constructor

    /// <param name="configuration">
    ///     Valid Key Sizes: 128
    ///     Valid Block Sizes: 8
    /// </param>
    public TripleDesCodec(BlockCipherConfiguration configuration)
    {
        if (configuration.GetKeySize() != KeySize._128)
            throw new ArgumentOutOfRangeException(nameof(configuration), $"Valid {nameof(KeySize)} values for {nameof(TripleDesCodec)} are {KeySize._128}");

        if (configuration.GetBlockSize() != BlockSize._8)
            throw new ArgumentOutOfRangeException(nameof(configuration), $"Valid {nameof(BlockSize)} values for {nameof(TripleDesCodec)} are {BlockSize._8}");

        _Preprocessor = configuration.GetPreprocessor();
        _CipherMode = configuration.GetBlockCipherMode();
        _PaddingMode = configuration.GetBlockPaddingMode();
        _KeySize = configuration.GetKeySize();
        _BlockSize = configuration.GetBlockSize();
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Decrypt
    /// </summary>
    /// <param name="encipherment"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public byte[] Decrypt(ReadOnlySpan<byte> encipherment, ReadOnlySpan<byte> key)
    {
        if ((encipherment.Length % _BlockSize) != 0)
            throw new InvalidOperationException($"the argument {nameof(encipherment)} was not padded using {_BlockSize} bytes");

        TripleDESCryptoServiceProvider desCryptoServiceProvider = GetDesProvider(key);

        using MemoryStream memoryStream = new(encipherment.ToArray());
        using CryptoStream cryptoStream = new(memoryStream, desCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Read);
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(encipherment.Length);

        byte[] result = new byte[encipherment.Length];

        cryptoStream.Read(result, 0, encipherment.Length);

        return result;
    }

    public BlockCipherAlgorithm GetAlgorithm() => BlockCipherAlgorithm.Aes;

    private TripleDESCryptoServiceProvider GetDesProvider(ReadOnlySpan<byte> key) =>
        new()
        {
            BlockSize = _BlockSize.GetBlockSize(),
            KeySize = _KeySize,
            Key = key.ToArray(),
            Mode = _CipherMode.AsCipherMode(),
            Padding = _PaddingMode.AsPaddingMode()
        };

    public KeySize GetKeySize() => _KeySize;

    public byte[] Sign(ReadOnlySpan<byte> message, ReadOnlySpan<byte> key)
    {
        TripleDESCryptoServiceProvider desCryptoServiceProvider = GetDesProvider(key);
        byte[] result = _Preprocessor.Preprocess(message);

        using MemoryStream memoryStream = new();
        using CryptoStream cryptoStream = new(memoryStream, desCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);

        cryptoStream.Write(result, 0, result.Length);

        return memoryStream.ToArray();
    }

    public BlockCipherMode GetCipherMode() => _CipherMode;

    #endregion
}