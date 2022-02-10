using Play.Core;
using Play.Core.Extensions;

namespace Play.Interchange.Messages.Header;

public sealed record Class : EnumObject<byte>
{
    #region Static Metadata

    public static readonly Class Authorization = new(1);
    public static readonly Class Financial = new(2);
    public static readonly Class FileActions = new(3);
    public static readonly Class Reversal = new(4);
    public static readonly Class Reconciliation = new(5);
    public static readonly Class Administration = new(6);
    public static readonly Class Fee = new(7);
    public static readonly Class Management = new(8);

    private static readonly Dictionary<byte, Class> _ValueMap = new()
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

    private Class(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool IsValid(byte value) => _ValueMap.ContainsKey(value);

    public static Class Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"No {nameof(Class)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(Class x, Class y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(Class left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Class right) => left == right._Value;
    public static implicit operator ushort(Class value) => value._Value;
    public static bool operator !=(Class left, byte right) => !(left == right);
    public static bool operator !=(byte left, Class right) => !(left == right);

    #endregion
}