using Play.Codecs;
using Play.Domain.ValueObjects;

using static Play.Randoms.Randomize;

namespace Play.Domain.Common.ValueObjects;

/// <summary>
///     This value object represents a simple string identifier that Aggregates have the ability to generate on creation
/// </summary>
public record SimpleStringId : ValueObject<string>, IEquatable<SimpleStringId>
{
    #region Constructor

    // Constructor for Entity Framework
    private SimpleStringId()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public SimpleStringId(string value) : base(value)
    {
        if (value.Length != 20)
            throw new ValueObjectException("The Id was an invalid length");

        if (!PlayCodec.AlphaNumericSpecialCodec.IsValid(value))
            throw new ValueObjectException($"The Id has an invalid character. The identifier must be in the {nameof(AlphaNumericSpecial)} format");
    }

    #endregion

    #region Operator Overrides

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>

    #endregion

    #region Operator Overrides

    public static implicit operator string(SimpleStringId value)
    {
        return value.Value;
    }

    #endregion

    #endregion
}