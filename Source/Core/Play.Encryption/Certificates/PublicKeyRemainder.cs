using System.Numerics;

namespace Play.Encryption.Certificates;

public record PublicKeyRemainder
{
    #region Instance Values

    private readonly BigInteger _Value;

    #endregion

    #region Constructor

    public PublicKeyRemainder()
    {
        _Value = 0;
    }

    public PublicKeyRemainder(ReadOnlySpan<byte> value)
    {
        _Value = new BigInteger(value.ToArray());
    }

    public PublicKeyRemainder(BigInteger value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public Span<byte> AsSpan() => _Value.ToByteArray(true).AsSpan();
    public int GetByteCount() => _Value.GetByteCount(true);
    public bool IsEmpty() => _Value == 0;

    #endregion
}