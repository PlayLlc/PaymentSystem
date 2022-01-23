using Play.Encryption.Encryption.Ciphers.Symmetric;

namespace Play.Encryption.Encryption;

/// <summary>
///     Encryption configuration used for a terminal implementation
/// </summary>
public class TerminalEncryptionConfiguration
{
    #region Instance Values

    private readonly BlockCipherConfiguration _AesConfiguration;
    private readonly BlockCipherConfiguration _TripleDesConfiguration;

    #endregion

    #region Constructor

    public TerminalEncryptionConfiguration(BlockCipherConfiguration tripleDesConfiguration, BlockCipherConfiguration aesConfiguration)
    {
        ValidateTripleDesConfiguration(tripleDesConfiguration);
        ValidateAesConfiguration(aesConfiguration);

        _TripleDesConfiguration = tripleDesConfiguration;
        _AesConfiguration = aesConfiguration;
    }

    #endregion

    #region Instance Members

    public BlockCipherConfiguration GetAesConfiguration()
    {
        return _AesConfiguration;
    }

    public BlockCipherConfiguration GetTripleDesConfiguration()
    {
        return _TripleDesConfiguration;
    }

    private void ValidateAesConfiguration(BlockCipherConfiguration configuration)
    {
        if (configuration.GetKeySize().GetBitSize() == KeySize._64)
        {
            throw new ArgumentOutOfRangeException(nameof(configuration),
                                                  $"Valid {nameof(KeySize)} values for {nameof(AesCodec)} are {KeySize._128}, {KeySize._192}, and {KeySize._256}");
        }

        if (configuration.GetBlockSize() != BlockSize._16)
        {
            throw new ArgumentOutOfRangeException(nameof(configuration),
                                                  $"Valid {nameof(BlockSize)} values for {nameof(TripleDesCodec)} are {BlockSize._8}");
        }
    }

    private void ValidateTripleDesConfiguration(BlockCipherConfiguration configuration)
    {
        if (configuration.GetKeySize() != KeySize._128)
        {
            throw new ArgumentOutOfRangeException(nameof(configuration),
                                                  $"Valid {nameof(KeySize)} values for {nameof(TripleDesCodec)} are {KeySize._128}");
        }

        if (configuration.GetBlockSize() != BlockSize._8)
        {
            throw new ArgumentOutOfRangeException(nameof(configuration),
                                                  $"Valid {nameof(BlockSize)} values for {nameof(TripleDesCodec)} are {BlockSize._8}");
        }
    }

    #endregion

    //...
}