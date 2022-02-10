using Play.Core;
using Play.Core.Extensions;

namespace Play.Interchange.Messages.Header;

public sealed record Version : EnumObject<byte>
{
    #region Static Metadata

    public static readonly Version Authorization = new(1);
    public static readonly Version Financial = new(2);
    public static readonly Version FileActions = new(3);
    public static readonly Version Reversal = new(4);
    public static readonly Version Reconciliation = new(5);
    public static readonly Version Administration = new(6);
    public static readonly Version Fee = new(7);
    public static readonly Version Management = new(8);

    private static readonly Dictionary<byte, Version> _ValueMap = new()
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

    private Version(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool IsValid(byte value) => _ValueMap.ContainsKey(value);

    public static Version Get(byte value)
    {
        const byte bitMask = 0b00111111;

        if (!_ValueMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"No {nameof(Version)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(Version x, Version y) => x.Equals(y);

    #endregion

    #region Operator Overrides

    public static bool operator ==(Version left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Version right) => left == right._Value;
    public static implicit operator ushort(Version value) => value._Value;
    public static bool operator !=(Version left, byte right) => !(left == right);
    public static bool operator !=(byte left, Version right) => !(left == right);

    #endregion
}