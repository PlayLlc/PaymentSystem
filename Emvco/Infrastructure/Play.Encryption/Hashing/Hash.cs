﻿namespace Play.Encryption.Encryption.Hashing;

public class Hash : IEquatable<Hash>, IEqualityComparer<Hash>
{
    #region Static Metadata

    public const byte Length = 20;

    #endregion

    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    public Hash(ReadOnlySpan<byte> value)
    {
        if (value.Length != Length)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"The argument {nameof(value)} must be {Length} bytes in length to instantiate a {nameof(Hash)} object");
        }

        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    public ReadOnlySpan<byte> AsReadOnlySpan()
    {
        return _Value;
    }

    public int GetByteCount()
    {
        return Length;
    }

    #endregion

    #region Equality

    public bool Equals(Hash? other)
    {
        if (other is null)
            return false;

        for (int i = 0; i < Length; i++)
        {
            if (_Value[i] != other._Value[i])
                return false;
        }

        return true;
    }

    public bool Equals(Hash? x, Hash? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override bool Equals(object? other)
    {
        return other is Hash hash && Equals(hash);
    }

    public int GetHashCode(Hash other)
    {
        return other.GetHashCode();
    }

    public override int GetHashCode()
    {
        const int hash = 1181;

        unchecked
        {
            int result = 0;

            for (int i = 0; i < Length; i++)
                result += hash * _Value[i].GetHashCode();

            return result;
        }
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(Hash left, Hash right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Hash left, Hash right)
    {
        return !left.Equals(right);
    }

    #endregion
}