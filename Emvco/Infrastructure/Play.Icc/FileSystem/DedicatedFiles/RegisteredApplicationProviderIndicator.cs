using System;

using Play.Codecs;
using Play.Core.Extensions;
using Play.Icc.Exceptions;

namespace Play.Icc.FileSystem.DedicatedFiles;

public readonly struct RegisteredApplicationProviderIndicator
{
    #region Static Metadata

    public const byte ByteCount = 5;

    #endregion

    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="IccProtocolException"></exception>
    public RegisteredApplicationProviderIndicator(ulong value)
    {
        if (value.GetMostSignificantByte() > ByteCount)
        {
            throw new IccProtocolException(new ArgumentOutOfRangeException(nameof(value),
                $"The value {value} exceeded that maximum byte count of: [{ByteCount}] bytes in length"));
        }

        _Value = value;
    }

    public RegisteredApplicationProviderIndicator(ReadOnlySpan<byte> value)
    {
        if (value.Length == ByteCount)
            _Value = PlayCodec.UnsignedIntegerCodec.DecodeToUInt64(value);

        if (value.Length < ByteCount)
            _Value = PlayCodec.UnsignedIntegerCodec.DecodeToUInt64(value.LeftPaddedArray(0x00, ByteCount - value.Length));

        _Value = PlayCodec.UnsignedIntegerCodec.DecodeToUInt64(value[..4]);
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => PlayCodec.UnsignedIntegerCodec.Encode(_Value);
    public int CompareTo(RegisteredApplicationProviderIndicator other) => _Value.CompareTo(other);
    public int GetByteCount() => ByteCount;

    #endregion

    #region Equality

    public bool Equals(RegisteredApplicationProviderIndicator other) => _Value == other._Value;
    public bool Equals(RegisteredApplicationProviderIndicator x, RegisteredApplicationProviderIndicator y) => x.Equals(y);

    public override bool Equals(object? obj) =>
        obj is RegisteredApplicationProviderIndicator registeredApplicationProviderIndicator
        && Equals(registeredApplicationProviderIndicator);

    public int GetHashCode(RegisteredApplicationProviderIndicator other) => other.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 208697;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(RegisteredApplicationProviderIndicator left, RegisteredApplicationProviderIndicator right) =>
        left._Value == right._Value;

    public static implicit operator ulong(RegisteredApplicationProviderIndicator value) => value._Value;

    public static bool operator !=(RegisteredApplicationProviderIndicator left, RegisteredApplicationProviderIndicator right) =>
        !(left == right);

    #endregion
}