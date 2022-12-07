using System.Security.Cryptography;

using Microsoft.Toolkit.HighPerformance.Buffers;

namespace Play.Encryption.Ciphers.Symmetric;

public class AesCodec : IBlockCipher
{
    #region Instance Values

    private readonly BlockSize _BlockSize;
    private readonly BlockCipherMode _CipherMode;
    private readonly KeySize _KeySize;
    private readonly BlockPaddingMode _PaddingMode;
    private readonly IPreprocessPlainText _PlainTextPreprocessor;
    private byte[] _InitializationVector;

    #endregion

    #region Constructor

    /// <param name="configuration">
    ///     Valid Key Sizes: 128, 192, 256
    ///     Valid Block Sizes: 16
    /// </param>
    public AesCodec(BlockCipherConfiguration configuration)
    {
        if (configuration.GetKeySize().GetBitSize() == KeySize._64)
        {
            throw new ArgumentOutOfRangeException(nameof(configuration),
                $"Valid {nameof(KeySize)} values for {nameof(AesCodec)} are {KeySize._128}, {KeySize._192}, and {KeySize._256}");
        }

        if (configuration.GetBlockSize() != BlockSize._16)
            throw new ArgumentOutOfRangeException(nameof(configuration), $"Valid {nameof(BlockSize)} values for {nameof(AesCodec)} are {BlockSize._8}");

        _PlainTextPreprocessor = configuration.GetPreprocessor();
        _CipherMode = configuration.GetBlockCipherMode();
        _PaddingMode = configuration.GetBlockPaddingMode();
        _KeySize = configuration.GetKeySize();
        _BlockSize = configuration.GetBlockSize();
        _InitializationVector = configuration.GetInitializationVector();
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

        AesCryptoServiceProvider desCryptoServiceProvider = GetAesProvider(key);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(encipherment.Length);
        using MemoryStream memoryStream = new(encipherment.ToArray());
        using CryptoStream cryptoStream = new(memoryStream, desCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Read);

        Span<byte> buffer = spanOwner.Span;
        encipherment.CopyTo(buffer);

        cryptoStream.Read(buffer);

        return buffer.ToArray();
    }

    private AesCryptoServiceProvider GetAesProvider(ReadOnlySpan<byte> key)
    {
        AesCryptoServiceProvider cryptoServiceProvider = new()
        {
            BlockSize = _BlockSize.GetBlockSize(),
            KeySize = _KeySize,
            Key = key.ToArray(),
            Mode = _CipherMode.AsCipherMode(),
            Padding = _PaddingMode.AsPaddingMode(),
        };

        if (_InitializationVector != null)
            cryptoServiceProvider.IV = _InitializationVector;

        return cryptoServiceProvider;
    }
        

    public BlockCipherAlgorithm GetAlgorithm() => BlockCipherAlgorithm.Aes;
    public KeySize GetKeySize() => _KeySize;

    public byte[] Encrypt(ReadOnlySpan<byte> message, ReadOnlySpan<byte> key)
    {
        AesCryptoServiceProvider provider = GetAesProvider(key);
        byte[] result = _PlainTextPreprocessor.Preprocess(message);

        using MemoryStream memoryStream = new();
        using CryptoStream cryptoStream = new(memoryStream, provider.CreateEncryptor(), CryptoStreamMode.Write);

        cryptoStream.Write(result, 0, result.Length);

        return memoryStream.ToArray();
    }

    public BlockCipherMode GetCipherMode() => _CipherMode;

    #endregion
}