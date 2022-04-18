using Play.Codecs;

namespace Play.Encryption.Certificates;

public readonly record struct CertificateSerialNumber
{
    #region Static Metadata

    private const byte _ByteCount = 3;

    #endregion

    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public CertificateSerialNumber(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteCount)
            throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(CertificateSerialNumber)} was expecting a value with {_ByteCount} bytes");

        _Value = PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(value);
    }

    #endregion

    #region Instance Members

    public byte[] Encode() => PlayCodec.UnsignedIntegerCodec.Encode(_Value);
    public int GetByteCount() => _ByteCount;

    #endregion
}