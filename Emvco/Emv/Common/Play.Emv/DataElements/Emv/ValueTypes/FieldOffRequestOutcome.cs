namespace Play.Emv.DataElements.Emv.ValueTypes;

/// <summary>
///     The kernel indicates whether to turn off the field (without card removal procedure). The hold time will
///     delay the processing of the next change to the field until it has elapsed.
/// </summary>
public readonly struct FieldOffRequestOutcome
{
    #region Static Metadata

    public static readonly FieldOffRequestOutcome NotAvailable;
    private const byte _BitOffset = (7 - 1) * 8;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static FieldOffRequestOutcome()
    {
        const byte notAvailable = 255;

        NotAvailable = new FieldOffRequestOutcome(notAvailable);
    }

    public FieldOffRequestOutcome(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) =>
        obj is FieldOffRequestOutcome outcomeParameterSetFieldOffRequest && Equals(outcomeParameterSetFieldOffRequest);

    public bool Equals(FieldOffRequestOutcome other) => _Value == other._Value;
    public bool Equals(FieldOffRequestOutcome x, FieldOffRequestOutcome y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 842417;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(FieldOffRequestOutcome left, FieldOffRequestOutcome right) => left._Value == right._Value;
    public static bool operator ==(FieldOffRequestOutcome left, byte right) => left._Value == right;
    public static bool operator ==(byte left, FieldOffRequestOutcome right) => left == right._Value;
    public static explicit operator ulong(FieldOffRequestOutcome value) => (ulong) (value._Value << _BitOffset);
    public static bool operator !=(FieldOffRequestOutcome left, FieldOffRequestOutcome right) => !(left == right);
    public static bool operator !=(FieldOffRequestOutcome left, byte right) => !(left == right);
    public static bool operator !=(byte left, FieldOffRequestOutcome right) => !(left == right);

    #endregion
}