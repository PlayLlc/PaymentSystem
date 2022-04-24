using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Core;

namespace Play.Icc.Messaging.Apdu;

public sealed record StatusWord1 : EnumObject<StatusWord>, IEqualityComparer<StatusWord>
{
    #region Static Metadata

    public static readonly StatusWord1 Empty = new();

    /// <summary>
    ///     Normal response indicating that response bytes are still available
    /// </summary>
    /// <values>Hexadecimal: 0x61</values>
    public static readonly StatusWord1 _61;

    /// <summary>
    ///     The non-volatile state of the ICC remains unchanged
    /// </summary>
    /// <values>Hexadecimal: 0x62</values>
    public static readonly StatusWord1 _62;

    /// <summary>
    ///     The non-volatile state of the ICC has changed
    /// </summary>
    /// <values>Hexadecimal: 0x63</values>
    public static readonly StatusWord1 _63;

    /// <summary>
    ///     The non-volatile state of the ICC remains unchanged, resulting in an error condition
    /// </summary>
    /// <values>Hexadecimal: 0x64</values>
    public static readonly StatusWord1 _64;

    /// <summary>
    ///     The non-volatile state of the ICC has changed, resulting in an error condition
    /// </summary>
    /// <values>Hexadecimal: 0x65</values>
    public static readonly StatusWord1 _65;

    /// <summary>
    ///     The command length, Lc, was incorrect in the Command APDU
    /// </summary>
    /// <values>Hexadecimal: 0x67</values>
    public static readonly StatusWord1 _67;

    /// <summary>
    ///     Functions in the Command APDU Class are not supported
    /// </summary>
    /// <values>Hexadecimal: 0x68</values>
    public static readonly StatusWord1 _68;

    /// <summary>
    ///     The Command APDU is not allowed
    /// </summary>
    /// <values>Hexadecimal: 0x69</values>
    public static readonly StatusWord1 _69;

    /// <summary>
    ///     The P1 or P2 parameters were incorrect
    /// </summary>
    /// <values>Hexadecimal: 0x6A</values>
    public static readonly StatusWord1 _6A;

    /// <summary>
    ///     The expected length, Le, was incorrect
    /// </summary>
    /// <values>Hexadecimal: 0x6C</values>
    public static readonly StatusWord1 _6C;

    /// <summary>
    ///     An internal exception occurred
    /// </summary>
    /// <values>Hexadecimal: 0x6F</values>
    public static readonly StatusWord1 _6F;

    private static readonly ImmutableSortedDictionary<StatusWord, StatusWord1> _ValueObjectMap;

    /// <summary>
    ///     The Status Word 1 value is unknown to this code base
    /// </summary>
    public static readonly StatusWord1 Unknown;

    #endregion

    #region Instance Values

    private readonly string _Description;
    private readonly StatusWordInfo _StatusWordInfo;

    #endregion

    #region Constructor

    public StatusWord1() : base()
    { }

    /// <exception cref="TypeInitializationException"></exception>
    static StatusWord1()
    {
        const byte unknown = 0;
        const byte responseBytesStillAvailable = 0x61;
        const byte nonVolatileMemoryUnchanged = 0x62;
        const byte nonVolatileMemoryChanged = 0x63;
        const byte nonVolatileMemoryUnchangedError = 0x64;
        const byte nonVolatileMemoryChangedError = 0x65;
        const byte wrongCommandLength = 0x67;
        const byte notSupported = 0x68;
        const byte commandNotAllowed = 0x69;
        const byte wrongParameters = 0x6A;
        const byte wrongExpectedLength = 0x6C;
        const byte internalException = 0x6F;

        Unknown = new StatusWord1(unknown, StatusWordInfo.Info, $"The {nameof(StatusWord1)} was not recognized in this code base");
        _61 = new StatusWord1(responseBytesStillAvailable, StatusWordInfo.Info, "Normal response indicating that response bytes are still available");
        _62 = new StatusWord1(nonVolatileMemoryUnchanged, StatusWordInfo.Warning, "The non-volatile state of the ICC remains unchanged");
        _63 = new StatusWord1(nonVolatileMemoryChanged, StatusWordInfo.Warning, "The non-volatile state of the ICC has changed");
        _64 = new StatusWord1(nonVolatileMemoryUnchangedError, StatusWordInfo.Error,
            "The non-volatile state of the ICC remains unchanged, resulting in an error condition");
        _65 = new StatusWord1(nonVolatileMemoryChangedError, StatusWordInfo.Error,
            "The non-volatile state of the ICC has changed, resulting in an error condition");
        _67 = new StatusWord1(wrongCommandLength, StatusWordInfo.Error, "The command length, Lc, was incorrect in the Command APDU");
        _68 = new StatusWord1(notSupported, StatusWordInfo.Warning, "Functions in the Command APDU Class are not supported");
        _69 = new StatusWord1(commandNotAllowed, StatusWordInfo.Error, "The Command APDU is not allowed");
        _6A = new StatusWord1(wrongParameters, StatusWordInfo.Error, "The P1 or P2 parameters were incorrect");
        _6C = new StatusWord1(wrongExpectedLength, StatusWordInfo.Error, "The expected length, Le, was incorrect");
        _6F = new StatusWord1(internalException, StatusWordInfo.Error, "An internal exception occurred");

        _ValueObjectMap = new Dictionary<StatusWord, StatusWord1>()
        {
            {Unknown, Unknown},
            {_61, _61},
            {_62, _62},
            {_63, _63},
            {_64, _64},
            {_65, _65},
            {_67, _67},
            {_68, _68},
            {_69, _69},
            {_6A, _6A},
            {_6C, _6C},
            {_6F, _6F}
        }.ToImmutableSortedDictionary();
    }

    private StatusWord1(StatusWord value, StatusWordInfo statusWordInfo, string description = "") : base(value)
    {
        _StatusWordInfo = statusWordInfo;
        _Description = description;
    }

    #endregion

    #region Instance Members

    public override StatusWord1[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(StatusWord value, out EnumObject<StatusWord>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out StatusWord1? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public string GetDescription() => _Description;
    public StatusWordInfo GetStatusWordInfo() => _StatusWordInfo;
    public static bool TryGet(StatusWord value, out StatusWord1? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(StatusWord1? other) => other is not null && (_Value == other._Value);

    public bool Equals(StatusWord1? x, StatusWord1? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(StatusWord1 other) => other.GetHashCode();
    public override int GetHashCode() => unchecked(63247 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static explicit operator byte(StatusWord1 statusWord1) => statusWord1._Value;

    #endregion
}