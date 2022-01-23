﻿using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Play.Core;

namespace Play.Icc.Emv;

internal record Instruction : EnumObject<byte>, IComparable<Instruction>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, Instruction> _ValueObjectMap;

    /// <value>Hexadecimal: 0x1E</value>
    public static readonly Instruction ApplicationBlock;

    /// <value>Hexadecimal: 0x18</value>
    public static readonly Instruction ApplicationUnblock;

    /// <value>Hexadecimal: 0x16</value>
    public static readonly Instruction CardBlock;

    /// <value>Hexadecimal: 0x2A</value>
    public static readonly Instruction ComputeCryptographicChecksum;

    /// <value>Hexadecimal: 0xA8</value>
    public static readonly Instruction GetProcessingOptions;

    #endregion

    #region Constructor

    static Instruction()
    {
        const byte getProcessingOptions = 0xA8;
        const byte applicationBlock = 0x1E;
        const byte applicationUnblock = 0x18;
        const byte cardBlock = 0x16;
        const byte computeCryptographicChecksum = 0x2A;

        GetProcessingOptions = new Instruction(getProcessingOptions);
        ApplicationBlock = new Instruction(applicationBlock);
        ApplicationUnblock = new Instruction(applicationUnblock);
        CardBlock = new Instruction(cardBlock);
        ComputeCryptographicChecksum = new Instruction(computeCryptographicChecksum);

        _ValueObjectMap = new Dictionary<byte, Instruction>
        {
            {getProcessingOptions, GetProcessingOptions}, {applicationBlock, ApplicationBlock},
            {applicationUnblock, ApplicationUnblock}, {cardBlock, CardBlock},
            {computeCryptographicChecksum, ComputeCryptographicChecksum}
        }.ToImmutableSortedDictionary();
    }

    private Instruction(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int CompareTo([AllowNull] Instruction other)
    {
        if (other is null)
            return 1;

        return _Value.CompareTo(other);
    }

    public static Instruction Get(byte value)
    {
        return _ValueObjectMap[value];
    }

    #endregion

    #region Equality

    public bool Equals(Instruction x, Instruction y)
    {
        return x.Equals(y);
    }

    public override int GetHashCode()
    {
        const int hash = 658379;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(Instruction left, byte right)
    {
        return left._Value == right;
    }

    public static bool operator ==(byte left, Instruction right)
    {
        return left == right._Value;
    }

    // logical channel values are from 0 to 3 so casting to sbyte will not truncate any meaningful information
    public static explicit operator sbyte(Instruction value)
    {
        return (sbyte) value._Value;
    }

    public static explicit operator short(Instruction value)
    {
        return value._Value;
    }

    public static explicit operator ushort(Instruction value)
    {
        return value._Value;
    }

    public static explicit operator int(Instruction value)
    {
        return value._Value;
    }

    public static explicit operator uint(Instruction value)
    {
        return value._Value;
    }

    public static explicit operator long(Instruction value)
    {
        return value._Value;
    }

    public static explicit operator ulong(Instruction value)
    {
        return value._Value;
    }

    public static implicit operator byte(Instruction value)
    {
        return value._Value;
    }

    public static bool operator !=(Instruction left, byte right)
    {
        return !(left == right);
    }

    public static bool operator !=(byte left, Instruction right)
    {
        return !(left == right);
    }

    #endregion
}