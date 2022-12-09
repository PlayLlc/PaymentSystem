﻿using System;
using System.Collections.Generic;
using System.Linq;

using Play.Core;

namespace Play.Ber.Tags;

public sealed record IdentifierType : EnumObject<byte>, IEqualityComparer<byte>
{
    #region Static Metadata

    public static readonly IdentifierType Empty = new();
    public static readonly IdentifierType LongIdentifier;
    public static readonly IdentifierType ShortIdentifier;
    private static readonly Dictionary<byte, IdentifierType> _ValueObjectMap = new() {{LongIdentifier, LongIdentifier}, {ShortIdentifier, ShortIdentifier}};

    #endregion

    #region Constructor

    public IdentifierType()
    { }

    /// <exception cref="TypeInitializationException"></exception>
    static IdentifierType()
    {
        const byte shortIdentifier = 0;
        const byte longIdentifier = 31;

        ShortIdentifier = new IdentifierType(shortIdentifier);
        LongIdentifier = new IdentifierType(longIdentifier);
    }

    private IdentifierType(byte value) : base(value)
    { }

    #endregion

    #region Equality

    public bool Equals(IdentifierType? other) => other is not null && (_Value == other._Value);

    public bool Equals(IdentifierType? x, IdentifierType? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IdentifierType other) => other.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 7354873;

        return hash + (_Value.GetHashCode() * 3);
    }

    #endregion

    #region Operator Overrides

    public static explicit operator byte(IdentifierType identifierType) => identifierType._Value;

    #endregion

    #region Instance Members

    public override IdentifierType[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out IdentifierType? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static bool TryGet(byte value, out IdentifierType? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion
}