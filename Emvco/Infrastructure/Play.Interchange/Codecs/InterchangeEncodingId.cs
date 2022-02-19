using Play.Emv.Interchange.Exceptions;

namespace Play.Interchange.Codecs;

public record InterchangeEncodingId
{
    #region Instance Values

    private readonly ulong _Id;
    private readonly string _FullyQualifiedName;

    #endregion

    #region Constructor

    internal InterchangeEncodingId(Type value)
    {
        if (!value.IsSubclassOf(typeof(IInterchangeDataFieldCodec)))
        {
            throw new InterchangeException(new ArgumentOutOfRangeException(
                $"The {nameof(InterchangeEncodingId)} can only be initialized if the argument {nameof(value)} is derived from {nameof(InterchangeDataFieldCodec)}"));
        }

        _Id = GetHashedId(value.FullName);
        _FullyQualifiedName = value.FullName!;
    }

    #endregion

    #region Instance Members

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

    public string GetFullyQualifiedName() => _FullyQualifiedName;
    public override string ToString() => _FullyQualifiedName;

    #endregion

    #region Operator Overrides

    public static implicit operator ulong(InterchangeEncodingId tag) => tag._Id;

    #endregion
}