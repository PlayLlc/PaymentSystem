using System.Diagnostics.CodeAnalysis;
using System.Numerics;

using Microsoft.Toolkit.HighPerformance.Buffers;

namespace Play.Encryption.Certificates;

public readonly struct PublicKeyModulus
{
    #region Static Metadata

    private const byte _MaxByteCount = 248;

    #endregion

    #region Instance Values

    private readonly BigInteger _Value;

    #endregion

    #region Constructor

    public PublicKeyModulus(byte[] value)
    {
        if (value.Length > _MaxByteCount)
            throw new ArgumentOutOfRangeException(nameof(value), $"The {nameof(PublicKeyModulus)} must be {_MaxByteCount} bytes or less");

        _Value = new BigInteger(value);
    }

    public PublicKeyModulus(ReadOnlySpan<byte> value)
    {
        if (value.Length > _MaxByteCount)
            throw new ArgumentOutOfRangeException(nameof(value), $"The {nameof(PublicKeyModulus)} must be {_MaxByteCount} bytes or less");

        _Value = new BigInteger(value);
    }

    #endregion

    #region Instance Members

    public BigInteger AsBigInteger() => _Value;
    public byte[] AsByteArray() => _Value.ToByteArray();
    public int GetByteCount() => _Value.GetByteCount();

    #endregion

    #region Equality

    public bool Equals(PublicKeyModulus other)
    {
        if (GetByteCount() != other.GetByteCount())
            return false;

        SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(GetByteCount());

        ReadOnlySpan<byte> thisBuffer = AsByteArray();
        ReadOnlySpan<byte> otherBuffer = other.AsByteArray();

        for (int i = 0; i < GetByteCount(); i++)
        {
            if (thisBuffer[i] != otherBuffer[i])
                return false;
        }

        return true;
    }

    public bool Equals(PublicKeyModulus x, PublicKeyModulus y) => x.Equals(y);
    public override bool Equals([AllowNull] object obj) => obj is PublicKeyModulus publicKeyModulus && Equals(publicKeyModulus);
    public int GetHashCode(PublicKeyModulus obj) => obj.GetHashCode();
    public override int GetHashCode() => unchecked(85691 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static bool operator ==(PublicKeyModulus left, PublicKeyModulus right) => left._Value == right._Value;
    public static bool operator >(PublicKeyModulus left, PublicKeyModulus right) => left._Value > right._Value;
    public static bool operator >=(PublicKeyModulus left, PublicKeyModulus right) => left._Value >= right._Value;
    public static bool operator !=(PublicKeyModulus left, PublicKeyModulus right) => !(left == right);
    public static bool operator <(PublicKeyModulus left, PublicKeyModulus right) => left._Value < right._Value;
    public static bool operator <=(PublicKeyModulus left, PublicKeyModulus right) => left._Value <= right._Value;

    #endregion
}