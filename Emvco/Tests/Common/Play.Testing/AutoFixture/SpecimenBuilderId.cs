using Play.Codecs;

namespace Play.Testing;

public readonly record struct SpecimenBuilderId
{
    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    public SpecimenBuilderId(ReadOnlySpan<char> value)
    {
        const uint hash = 13;
        uint result = 0;

        unchecked
        {
            foreach (byte encoding in PlayCodec.UnicodeCodec.Encode(value))
                result += encoding * hash;
        }

        _Value = result;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator uint(SpecimenBuilderId id) => id._Value;

    #endregion
}