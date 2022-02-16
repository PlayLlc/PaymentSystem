using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Ber.Exceptions;
using Play.Core;
using Play.Core.Extensions;

namespace Play.Ber.Identifiers;

public sealed record ClassType : EnumObject<byte>, IEqualityComparer<ClassType>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, ClassType> _ValueObjectMap;

    /// <remarks>DecimalValue: 64; HexValue: 0x40</remarks>
    public static readonly ClassType Application;

    /// <remarks>DecimalValue: 128; HexValue: 0x80</remarks>
    public static readonly ClassType ContextSpecific;

    /// <remarks>DecimalValue: 192; HexValue: 0xC0</remarks>
    public static readonly ClassType Private;

    /// <remarks>DecimalValue: 0; HexValue: 0x0</remarks>
    public static readonly ClassType Universal;

    private const byte _Universal = 0;
    private const byte _Application = (byte) Bits.Seven;
    private const byte _ContextSpecific = (byte) Bits.Eight;
    private const byte _Private = (byte) (Bits.Eight | Bits.Seven);
    internal const byte UnrelatedBits = (byte) (Bits.Six | Bits.Five | Bits.Four | Bits.Three | Bits.Two | Bits.One);

    #endregion

    #region Constructor

    /// <exception cref="TypeInitializationException"></exception>
    static ClassType()
    {
        Application = new ClassType(_Application);
        ContextSpecific = new ClassType(_ContextSpecific);
        Private = new ClassType(_Private);
        Universal = new ClassType(_Universal);

        _ValueObjectMap = new Dictionary<byte, ClassType>
        {
            {_Application, Application}, {_ContextSpecific, ContextSpecific}, {_Private, Private}, {_Universal, Universal}
        }.ToImmutableSortedDictionary();
    }

    private ClassType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool TryGet(byte value, out ClassType result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(ClassType? other) => other is not null && (other._Value == _Value);

    public bool Equals(ClassType? x, ClassType? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ClassType other) => other.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 7354873;

        return hash + (_Value.GetHashCode() * 3);
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(ClassType left, byte right)
    {
        if (left is null)
            return false;

        return left._Value == right;
    }

    public static bool operator ==(byte left, ClassType right)
    {
        if (right is null)
            return false;

        return right._Value == left;
    }

    public static explicit operator byte(ClassType classType) => classType._Value;

    public static explicit operator ClassType(byte classType)
    {
        if (!TryGet(classType, out ClassType result))
        {
            throw new BerFormatException(new ArgumentOutOfRangeException(nameof(classType),
                $"The {nameof(ClassType)} could not be found from the number supplied to the argument: {classType}"));
        }

        return result;
    }

    public static bool operator !=(ClassType left, byte right) => !(left == right);
    public static bool operator !=(byte left, ClassType right) => !(left == right);

    #endregion
}