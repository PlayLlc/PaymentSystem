using Play.Codecs;

namespace Play.Emv.Ber.TestData.AutoFixture;

public readonly record struct SpecimenBuilderId
{
    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    public SpecimenBuilderId(ReadOnlySpan<char> value)
    {
        _Value = PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(PlayCodec.UnicodeCodec.Encode(value));
    }

    #endregion

    #region Operator Overrides

    public static implicit operator uint(SpecimenBuilderId id) => id._Value;

    #endregion
}