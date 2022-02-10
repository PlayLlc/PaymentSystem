using Play.Core;
using Play.Core.Extensions;

namespace Play.Interchange.Messages.Header;

public sealed record Function : EnumObject<byte>
{
    #region Static Metadata

    public static readonly Function Authorization = new(1);
    public static readonly Function Financial = new(2);
    public static readonly Function FileActions = new(3);
    public static readonly Function Reversal = new(4);
    public static readonly Function Reconciliation = new(5);
    public static readonly Function Administration = new(6);
    public static readonly Function Fee = new(7);
    public static readonly Function Management = new(8);

    private static readonly Dictionary<byte, Function> _ValueMap = new()
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

    private Function(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool IsValid(byte value) => _ValueMap.ContainsKey(value);

    public static Function Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"No {nameof(Function)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(Function x, Function y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(Function left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Function right) => left == right._Value;
    public static implicit operator ushort(Function value) => value._Value;
    public static bool operator !=(Function left, byte right) => !(left == right);
    public static bool operator !=(byte left, Function right) => !(left == right);

    #endregion
}