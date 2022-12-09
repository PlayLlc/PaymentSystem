﻿using System;

using Play.Core.Extensions;

namespace Play.Core;

/// <summary>
///     This object represents 4 bits. It's the equivalent to the right half of a byte
/// </summary>
public readonly record struct Nibble
{
    #region Static Metadata

    public static readonly Nibble MaxValue = new(0xF);
    public static readonly Nibble MinValue = new(0);
    private const byte _UnrelatedBits = 0xF0;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    /// <param name="value"></param>
    /// <warning>The max value is 0x0F</warning>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Nibble(byte value)
    {
        if (value.AreAnyBitsSet(_UnrelatedBits))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The {nameof(Nibble)} could not be initialized because the argument provided has more bits set than a nibble");
        }

        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator byte(Nibble value) => value._Value;
    public static implicit operator Nibble(byte value) => new(value);

    #endregion
}