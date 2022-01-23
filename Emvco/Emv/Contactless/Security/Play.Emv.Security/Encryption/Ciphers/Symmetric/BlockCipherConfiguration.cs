namespace Play.Emv.Security.Encryption.Ciphers;

public class BlockCipherConfiguration
{
    #region Instance Values

    private readonly BlockSize _BlockSize;
    private readonly BlockCipherMode _CipherMode;
    private readonly KeySize _KeySize;
    private readonly BlockPaddingMode _PaddingMode;
    private readonly IPreprocessPlainText? _Preprocessor;

    #endregion

    #region Constructor

    public BlockCipherConfiguration(
        BlockCipherMode cipherMode,
        BlockPaddingMode paddingMode,
        KeySize keySize,
        BlockSize blockSize,
        IPreprocessPlainText? preprocessor)
    {
        _CipherMode = cipherMode;
        _PaddingMode = paddingMode;
        _KeySize = keySize;
        _BlockSize = blockSize;
        _Preprocessor = preprocessor;
    }

    #endregion

    #region Instance Members

    public BlockCipherMode GetBlockCipherMode() => _CipherMode;
    public BlockPaddingMode GetBlockPaddingMode() => _PaddingMode;
    public BlockSize GetBlockSize() => _BlockSize;
    public KeySize GetKeySize() => _KeySize;
    public IPreprocessPlainText GetPreprocessor() => _Preprocessor ?? new DefaultPlainTextPreprocessor();

    #endregion
}