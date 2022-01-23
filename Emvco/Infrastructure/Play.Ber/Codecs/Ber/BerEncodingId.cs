using System;

using Play.Ber.Exceptions;
using Play.Core.Exceptions;

namespace Play.Ber.Codecs;

public readonly record struct BerEncodingId
{
    #region Instance Values

    private readonly string _FullyQualifiedName;
    private readonly ulong _Id;

    #endregion

    #region Constructor

    internal BerEncodingId(Type value)
    { 

        if (!value.IsSubclassOf(typeof(BerPrimitiveCodec)))
        {
            throw new BerFormatException(new ArgumentOutOfRangeException(
                $"The {nameof(BerEncodingId)} can only be initialized if the argument {nameof(value)} is derived from {nameof(BerPrimitiveCodec)}"));
        }

        _FullyQualifiedName = value.FullName!;
        _Id = GetHashedId(_FullyQualifiedName);
    }

    #endregion

    #region Instance Members

    public string GetFullyQualifiedName() => _FullyQualifiedName;

    private static ulong GetHashedId(ReadOnlySpan<char> value)
    {
        const ulong hash = 31489;

        ulong result = 0;

        unchecked
        {
            for (int i = 0; i < value.Length; i++)
                result += (ulong) value[i].GetHashCode() * hash;
        }

        return result;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator ulong(BerEncodingId tag) => tag._Id;

    #endregion
}