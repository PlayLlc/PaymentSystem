using System;

using Play.Codecs;
using Play.Core.Exceptions;
using Play.Icc.Exceptions;

namespace Play.Icc.FileSystem;

internal class FilePath
{
    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    public FilePath(params FileIdentifier[] fileIdentifiers)
    {
        CheckCore.ForNullOrEmptySequence(fileIdentifiers, nameof(fileIdentifiers));

        Span<byte> result = stackalloc byte[fileIdentifiers.Length * 2];

        for (int i = 0, j = 0; i < fileIdentifiers.Length; i++, j += 2)
            fileIdentifiers[i].AsByteArray().CopyTo(result[j..]);

        _Value = result.ToArray();
    }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="IccProtocolException"></exception>
    public FilePath(ReadOnlySpan<byte> value)
    {
        if ((value.Length % 2) != 0)
        {
            throw new IccProtocolException(new ArgumentOutOfRangeException(
                $"The {nameof(FilePath)} is a concatenation of {nameof(FileIdentifier)}, which must be two bytes in length each. The byte sequence provided was out of range because it was an odd length"));
        }

        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    public bool DoesPathStartFromRootDirectory() => FileIdentifier.MasterFile == new FileIdentifier(PlayCodec.UnsignedIntegerCodec.DecodeToUInt16(_Value[..2]));

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is FilePath flePath && Equals(flePath);

    public bool Equals(FilePath? other)
    {
        if (other is null)
            return false;

        return _Value == other!._Value;
    }

    public bool Equals(FilePath x, FilePath y) => x.Equals(y);

    public bool Equals(byte[] other)
    {
        CheckCore.ForNullOrEmptySequence(other, nameof(other));

        for (int i = 0; i < other.Length; i++)
        {
            if (other[i] != _Value[i])
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        const int hash = 10544431;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(FilePath left, FilePath right) => left._Value == right._Value;
    public static bool operator ==(FilePath left, byte[] right) => left.Equals(right);
    public static bool operator ==(byte[] left, FilePath right) => right.Equals(left);

    public static implicit operator byte[](FilePath value)
    {
        CheckCore.ForEmptySequence(value._Value, nameof(value));

        return value._Value;
    }

    public static implicit operator ReadOnlySpan<byte>(FilePath value)
    {
        CheckCore.ForEmptySequence(value._Value, nameof(value));

        return value._Value;
    }

    public static bool operator !=(FilePath left, FilePath right) => !(left == right);
    public static bool operator !=(FilePath left, byte[] right) => !left.Equals(right);
    public static bool operator !=(byte[] left, FilePath right) => !right.Equals(left);

    #endregion
}