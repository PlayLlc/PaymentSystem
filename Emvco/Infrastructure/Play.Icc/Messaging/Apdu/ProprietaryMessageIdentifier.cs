namespace Play.Icc.Messaging.Apdu;

public readonly struct ProprietaryMessageIdentifier
{
    #region Static Metadata

    public static readonly ProprietaryMessageIdentifier _8x;
    public static readonly ProprietaryMessageIdentifier _9x;
    public static readonly ProprietaryMessageIdentifier Dx;
    public static readonly ProprietaryMessageIdentifier Ex;
    public static readonly ProprietaryMessageIdentifier Fx;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static ProprietaryMessageIdentifier()
    {
        const byte eightX = 0x80;
        const byte nineX = 0x90;
        const byte dX = 0xD0;
        const byte eX = 0xE0;
        const byte fX = 0xF0;

        _8x = new ProprietaryMessageIdentifier(eightX);
        _9x = new ProprietaryMessageIdentifier(nineX);
        Dx = new ProprietaryMessageIdentifier(dX);
        Ex = new ProprietaryMessageIdentifier(eX);
        Fx = new ProprietaryMessageIdentifier(fX);
    }

    private ProprietaryMessageIdentifier(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) =>
        obj is ProprietaryMessageIdentifier proprietaryCommandResponsePair && Equals(proprietaryCommandResponsePair);

    public bool Equals(ProprietaryMessageIdentifier other) => _Value == other._Value;
    public bool Equals(ProprietaryMessageIdentifier x, ProprietaryMessageIdentifier y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 658379;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(ProprietaryMessageIdentifier left, ProprietaryMessageIdentifier right) => left._Value == right._Value;
    public static bool operator ==(ProprietaryMessageIdentifier left, byte right) => left._Value == right;
    public static bool operator ==(byte left, ProprietaryMessageIdentifier right) => left == right._Value;

    // logical channel values are from 0 to 3 so casting to sbyte will not truncate any meaningful information
    public static explicit operator sbyte(ProprietaryMessageIdentifier value) => (sbyte) value._Value;
    public static explicit operator short(ProprietaryMessageIdentifier value) => value._Value;
    public static explicit operator ushort(ProprietaryMessageIdentifier value) => value._Value;
    public static explicit operator int(ProprietaryMessageIdentifier value) => value._Value;
    public static explicit operator uint(ProprietaryMessageIdentifier value) => value._Value;
    public static explicit operator long(ProprietaryMessageIdentifier value) => value._Value;
    public static explicit operator ulong(ProprietaryMessageIdentifier value) => value._Value;
    public static implicit operator byte(ProprietaryMessageIdentifier value) => value._Value;
    public static bool operator !=(ProprietaryMessageIdentifier left, ProprietaryMessageIdentifier right) => !(left == right);
    public static bool operator !=(ProprietaryMessageIdentifier left, byte right) => !(left == right);
    public static bool operator !=(byte left, ProprietaryMessageIdentifier right) => !(left == right);

    #endregion
}