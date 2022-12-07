using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Core;

namespace Play.Icc.Messaging.Apdu;

public record ProprietaryMessageIdentifier : EnumObject<byte>
{
    #region Static Metadata

    public static readonly ProprietaryMessageIdentifier Empty = new();
    public static readonly ProprietaryMessageIdentifier _8x;
    public static readonly ProprietaryMessageIdentifier _9x;
    public static readonly ProprietaryMessageIdentifier Dx;
    public static readonly ProprietaryMessageIdentifier Ex;
    public static readonly ProprietaryMessageIdentifier Fx;
    private static readonly ImmutableSortedDictionary<byte, ProprietaryMessageIdentifier> _ValueObjectMap;

    #endregion

    #region Constructor

    public ProprietaryMessageIdentifier()
    { }

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

        _ValueObjectMap =
            new Dictionary<byte, ProprietaryMessageIdentifier> {{_8x, _8x}, {_9x, _9x}, {Dx, Dx}, {Ex, Ex}, {Fx, Fx}}.ToImmutableSortedDictionary();
    }

    private ProprietaryMessageIdentifier(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ProprietaryMessageIdentifier[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out ProprietaryMessageIdentifier? innerResult))
        {
            result = innerResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion

    #region Equality

    public bool Equals(ProprietaryMessageIdentifier x, ProprietaryMessageIdentifier y) => x.Equals(y);

    public override int GetHashCode()
    {
        const int hash = 658379;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

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
    public static bool operator !=(ProprietaryMessageIdentifier left, byte right) => !(left == right);
    public static bool operator !=(byte left, ProprietaryMessageIdentifier right) => !(left == right);

    #endregion
}