using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Ber.Exceptions;
using Play.Core;
using Play.Core.Extensions;

namespace Play.Ber.Identifiers;

public sealed record ClassTypes : EnumObject<byte>, IEqualityComparer<byte>
{
    #region Static Metadata

    public static readonly ClassTypes Empty = new();
    private static readonly ImmutableSortedDictionary<byte, ClassTypes> _ValueObjectMap;

    /// <remarks>DecimalValue: 64; HexValue: 0x40</remarks>
    public static readonly ClassTypes Application;

    /// <remarks>DecimalValue: 128; HexValue: 0x80</remarks>
    public static readonly ClassTypes ContextSpecific;

    /// <remarks>DecimalValue: 192; HexValue: 0xC0</remarks>
    public static readonly ClassTypes Private;

    /// <remarks>DecimalValue: 0; HexValue: 0x0</remarks>
    public static readonly ClassTypes Universal;

    private const byte _Universal = 0;
    private const byte _Application = (byte) Bits.Seven;
    private const byte _ContextSpecific = (byte) Bits.Eight;
    private const byte _Private = (byte) (Bits.Eight | Bits.Seven);
    internal const byte UnrelatedBits = (byte) (Bits.Six | Bits.Five | Bits.Four | Bits.Three | Bits.Two | Bits.One);

    #endregion

    #region Constructor

    public ClassTypes()
    { }

    /// <exception cref="TypeInitializationException"></exception>
    static ClassTypes()
    {
        Application = new ClassTypes(_Application);
        ContextSpecific = new ClassTypes(_ContextSpecific);
        Private = new ClassTypes(_Private);
        Universal = new ClassTypes(_Universal);

        _ValueObjectMap = new Dictionary<byte, ClassTypes>
        {
            {_Application, Application}, {_ContextSpecific, ContextSpecific}, {_Private, Private}, {_Universal, Universal}
        }.ToImmutableSortedDictionary();
    }

    private ClassTypes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ClassTypes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out ClassTypes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static bool IsUniversal(byte tag) => tag.GetMaskedValue(UnrelatedBits).AreAnyBitsSet(0xFF);
    public static bool IsApplicationSpecific(byte tag) => tag.GetMaskedValue(UnrelatedBits) == _Application;
    public static bool TryGet(byte value, out ClassTypes result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(ClassTypes? other) => other is not null && (other._Value == _Value);

    public bool Equals(ClassTypes? x, ClassTypes? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ClassTypes other) => other.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 7354873;

        return hash + (_Value.GetHashCode() * 3);
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(ClassTypes left, byte right)
    {
        if (left is null)
            return false;

        return left._Value == right;
    }

    public static bool operator ==(byte left, ClassTypes right)
    {
        if (right is null)
            return false;

        return right._Value == left;
    }

    public static explicit operator byte(ClassTypes classTypes) => classTypes._Value;

    public static explicit operator ClassTypes(byte classType)
    {
        if (!TryGet(classType, out ClassTypes result))
        {
            throw new BerParsingException(new ArgumentOutOfRangeException(nameof(classType),
                $"The {nameof(ClassTypes)} could not be found from the number supplied to the argument: {classType}"));
        }

        return result;
    }

    public static bool operator !=(ClassTypes left, byte right) => !(left == right);
    public static bool operator !=(byte left, ClassTypes right) => !(left == right);

    #endregion
}