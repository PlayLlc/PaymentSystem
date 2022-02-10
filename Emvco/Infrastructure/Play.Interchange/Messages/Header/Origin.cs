using Play.Core;
using Play.Core.Extensions;

namespace Play.Interchange.Messages.Header;

public sealed record Origin : EnumObject<byte>
{
    #region Static Metadata

    public static readonly Origin Authorization = new(1);
    public static readonly Origin Financial = new(2);
    public static readonly Origin FileActions = new(3);
    public static readonly Origin Reversal = new(4);
    public static readonly Origin Reconciliation = new(5);
    public static readonly Origin Administration = new(6);
    public static readonly Origin Fee = new(7);
    public static readonly Origin Management = new(8);

    private static readonly Dictionary<byte, Origin> _ValueMap = new()
    {
        {Authorization, Authorization},
        {Financial, Financial},
        {FileActions, FileActions},
        {Reversal, Reversal},
        {Reconciliation, Reconciliation},
        {Administration, Administration},
        {Fee, Fee},
        {Management, Management}
    };

    #endregion

    #region Constructor

    private Origin(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool IsValid(byte value) => _ValueMap.ContainsKey(value);

    public static Origin Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"No {nameof(Origin)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(Origin x, Origin y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(Origin left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Origin right) => left == right._Value;
    public static implicit operator ushort(Origin value) => value._Value;
    public static bool operator !=(Origin left, byte right) => !(left == right);
    public static bool operator !=(byte left, Origin right) => !(left == right);

    #endregion
}