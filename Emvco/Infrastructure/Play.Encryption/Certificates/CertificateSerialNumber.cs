using System.Diagnostics.CodeAnalysis;
using System.Numerics;

using Microsoft.Toolkit.HighPerformance.Buffers;

namespace Play.Encryption.Certificates;

public readonly struct CertificateSerialNumber
{
    #region Static Metadata

    private const byte _ByteCount = 3;

    #endregion

    #region Instance Values

    private readonly BigInteger _Value;

    #endregion

    #region Constructor

    public CertificateSerialNumber(byte[] value)
    {
        if (value.Length != _ByteCount)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"{nameof(CertificateSerialNumber)} was expecting a value with {_ByteCount} bytes");
        }

        _Value = new BigInteger(value);
    }

    public CertificateSerialNumber(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteCount)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"{nameof(CertificateSerialNumber)} was expecting a value with {_ByteCount} bytes");
        }

        _Value = new BigInteger(value);
    }

    #endregion

    #region Instance Members

    public BigInteger AsBigInteger() => _Value;
    public byte[] AsByteArray() => _Value.ToByteArray();
    public int GetByteCount() => _Value.GetByteCount();

    #endregion

    #region Equality

    public bool Equals(CertificateSerialNumber other)
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

    public bool Equals(CertificateSerialNumber x, CertificateSerialNumber y) => x.Equals(y);

    public override bool Equals([AllowNull] object obj) =>
        obj is CertificateSerialNumber publicKeySerialNumber && Equals(publicKeySerialNumber);

    public int GetHashCode(CertificateSerialNumber obj) => obj.GetHashCode();
    public override int GetHashCode() => unchecked(85691 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static bool operator ==(CertificateSerialNumber left, CertificateSerialNumber right) => left._Value == right._Value;
    public static bool operator >(CertificateSerialNumber left, CertificateSerialNumber right) => left._Value > right._Value;
    public static bool operator >=(CertificateSerialNumber left, CertificateSerialNumber right) => left._Value >= right._Value;
    public static bool operator !=(CertificateSerialNumber left, CertificateSerialNumber right) => !(left == right);
    public static bool operator <(CertificateSerialNumber left, CertificateSerialNumber right) => left._Value < right._Value;
    public static bool operator <=(CertificateSerialNumber left, CertificateSerialNumber right) => left._Value <= right._Value;

    #endregion
}