using System;

using Play.Core.Exceptions;

namespace Play.Icc.FileSystem.DedicatedFiles;

/// <summary>
///     File containing file control information and optionally memory available for allocation. It may be the parent of
///     EFs
///     and/or DFs.
/// </summary>
internal readonly struct ApplicationIdentifier
{
    #region Static Metadata

    private const byte _MinLength = 5;
    private const byte _MaxLength = 16;

    #endregion

    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    public ApplicationIdentifier(ReadOnlySpan<byte> value)
    {
        Validate(value);
        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    internal RegisteredApplicationProviderIndicator GetRegisteredApplicationProviderIdentifier() => new(_Value[..4]);

    internal bool TryGetProprietaryApplicationIdentifier(out byte[] pixResult)
    {
        if (_Value.Length > 5)
        {
            pixResult = _Value[5.._Value.Length];

            return true;
        }

        pixResult = Array.Empty<byte>();

        return false;
    }

    private static void Validate(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (value.Length < _MinLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The argument {nameof(value)} was out of range. ApplicationIdentifier (AID) must be at least 5 bytes in length");
        }

        if (value.Length < _MaxLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The argument {nameof(value)} was out of range. ApplicationIdentifier (AID) must be less than 16 bytes in length");
        }
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is ApplicationIdentifier applicationIdentifier && Equals(applicationIdentifier);
    public bool Equals(ApplicationIdentifier other) => _Value == other._Value;
    public bool Equals(ApplicationIdentifier x, ApplicationIdentifier y) => x.Equals(y);

    public bool Equals(ReadOnlySpan<byte> other)
    {
        if (other.Length == 0)
            return false;

        if (other.Length != _Value.Length)
            return false;

        for (int i = 0; i < other.Length; i++)
        {
            if (other[i] != _Value[i])
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        const int hash = 9985351;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(ApplicationIdentifier left, ApplicationIdentifier right) => left._Value == right._Value;
    public static bool operator ==(ApplicationIdentifier left, ReadOnlySpan<byte> right) => left._Value == right;
    public static bool operator ==(ReadOnlySpan<byte> left, ApplicationIdentifier right) => left == right._Value;

    public static implicit operator byte[](ApplicationIdentifier value)
    {
        CheckCore.ForEmptySequence(value._Value, nameof(value));

        return value._Value;
    }

    public static implicit operator ReadOnlySpan<byte>(ApplicationIdentifier value)
    {
        CheckCore.ForEmptySequence(value._Value, nameof(value));

        return value._Value;
    }

    public static bool operator !=(ApplicationIdentifier left, ApplicationIdentifier right) => !(left == right);
    public static bool operator !=(ApplicationIdentifier left, ReadOnlySpan<byte> right) => !(left == right);
    public static bool operator !=(ReadOnlySpan<byte> left, ApplicationIdentifier right) => !(left == right);

    #endregion
}