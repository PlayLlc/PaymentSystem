using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Ber.Exceptions;
using Play.Core;
using Play.Core.Extensions;

namespace Play.Ber.Identifiers;

public sealed record DataObjectTypes : EnumObject<byte>, IEqualityComparer<byte>
{
    #region Static Metadata

    public static readonly DataObjectTypes Empty = new();
    private static readonly ImmutableSortedDictionary<byte, DataObjectTypes> _ValueObjectMap;
    public static readonly DataObjectTypes Constructed;
    public static readonly DataObjectTypes Primitive;
    private const byte _Primitive = 0;
    private const byte _Constructed = (byte) Bits.Six;

    #endregion

    #region Constructor

    public DataObjectTypes()
    { }

    /// <exception cref="TypeInitializationException"></exception>
    static DataObjectTypes()
    {
        Primitive = new DataObjectTypes(_Primitive);
        Constructed = new DataObjectTypes(_Constructed);

        _ValueObjectMap = new Dictionary<byte, DataObjectTypes> {{_Primitive, Primitive}, {_Constructed, Constructed}}.ToImmutableSortedDictionary();
    }

    private DataObjectTypes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataObjectTypes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out DataObjectTypes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public bool IsPrimitive() => _Value == _Primitive;
    public static bool TryGet(byte value, out DataObjectTypes? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(DataObjectTypes? other) => other is not null && (_Value == other._Value);

    public bool Equals(DataObjectTypes? x, DataObjectTypes? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DataObjectTypes other) => other.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 7354873;

        return hash + (_Value.GetHashCode() * 3);
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(DataObjectTypes left, byte right)
    {
        if (left is null)
            return false;

        return left._Value == right;
    }

    public static bool operator ==(byte left, DataObjectTypes right)
    {
        if (right is null)
            return false;

        return right._Value == left;
    }

    public static explicit operator byte(DataObjectTypes dataObjectTypes) => dataObjectTypes._Value;

    public static explicit operator DataObjectTypes(byte classType)
    {
        if (!TryGet(classType, out DataObjectTypes result))
        {
            throw new BerParsingException(new ArgumentOutOfRangeException(nameof(classType),
                $"The {nameof(DataObjectTypes)} could not be found from the number supplied to the argument: {classType}"));
        }

        return result;
    }

    public static bool operator !=(DataObjectTypes left, byte right) => !(left == right);
    public static bool operator !=(byte left, DataObjectTypes right) => !(left == right);

    #endregion
}