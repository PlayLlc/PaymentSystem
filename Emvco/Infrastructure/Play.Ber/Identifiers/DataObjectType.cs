using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Ber.Exceptions;
using Play.Core;
using Play.Core.Extensions;

namespace Play.Ber.Identifiers;

public sealed record DataObjectType : EnumObject<byte>, IEqualityComparer<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, DataObjectType> _ValueObjectMap;
    public static readonly DataObjectType Constructed;
    public static readonly DataObjectType Primitive;
    private const byte _Primitive = 0;
    private const byte _Constructed = (byte) Bits.Six;

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static DataObjectType()
    {
        Primitive = new DataObjectType(_Primitive);
        Constructed = new DataObjectType(_Constructed);

        _ValueObjectMap = new Dictionary<byte, DataObjectType> {{_Primitive, Primitive}, {_Constructed, Constructed}}.ToImmutableSortedDictionary();
    }

    private DataObjectType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataObjectType[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out DataObjectType? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static DataObjectType[] GetAll() => _ValueObjectMap.Values.ToArray();
    public bool IsPrimitive() => _Value == _Primitive;
    public static bool TryGet(byte value, out DataObjectType? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(DataObjectType? other) => other is not null && (_Value == other._Value);

    public bool Equals(DataObjectType? x, DataObjectType? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DataObjectType other) => other.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 7354873;

        return hash + (_Value.GetHashCode() * 3);
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(DataObjectType left, byte right)
    {
        if (left is null)
            return false;

        return left._Value == right;
    }

    public static bool operator ==(byte left, DataObjectType right)
    {
        if (right is null)
            return false;

        return right._Value == left;
    }

    public static explicit operator byte(DataObjectType dataObjectType) => dataObjectType._Value;

    public static explicit operator DataObjectType(byte classType)
    {
        if (!TryGet(classType, out DataObjectType result))
        {
            throw new BerParsingException(new ArgumentOutOfRangeException(nameof(classType),
                $"The {nameof(DataObjectType)} could not be found from the number supplied to the argument: {classType}"));
        }

        return result;
    }

    public static bool operator !=(DataObjectType left, byte right) => !(left == right);
    public static bool operator !=(byte left, DataObjectType right) => !(left == right);

    #endregion
}