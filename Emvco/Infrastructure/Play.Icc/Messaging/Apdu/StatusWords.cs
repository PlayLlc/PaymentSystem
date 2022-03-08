using System;
using System.Collections.Generic;

using Play.Icc.Exceptions;

namespace Play.Icc.Messaging.Apdu;

public class StatusWords : IEquatable<StatusWords>, IEqualityComparer<StatusWords>
{
    #region Static Metadata

    /// <summary>Selected file invalidated</summary>
    public static readonly StatusWords _6283;

    /// <summary>Wrong length</summary>
    public static readonly StatusWords _6700;

    /// <summary>Conditions of use not satisfied</summary>
    public static readonly StatusWords _6985;

    /// <summary>Function not supported</summary>
    public static readonly StatusWords _6A81;

    /// <summary>File not found</summary>
    public static readonly StatusWords _6A82;

    /// <summary>Normal processing</summary>
    public static readonly StatusWords _6A86;

    /// <summary>Command successfully executed (OK)</summary>
    public static readonly StatusWords _9000;

    public static readonly StatusWords NotAvailable;

    #endregion

    #region Instance Values

    private readonly byte _StatusWord1;
    private readonly byte _StatusWord2;

    #endregion

    #region Constructor

    static StatusWords()
    {
        NotAvailable = new StatusWords(0x00, 0x00);
        _9000 = new StatusWords(0x90, 0x00);
        _6283 = new StatusWords(0x62, 0x83);
        _6700 = new StatusWords(0x67, 0x00);
        _6A81 = new StatusWords(0x6A, 0x81);
        _6A82 = new StatusWords(0x6A, 0x82);
        _6A86 = new StatusWords(0x6A, 0x86);
        _6985 = new StatusWords(0x6A, 0x86);
    }

    public StatusWords(StatusWord statusWord1, StatusWord statusWord2)
    {
        _StatusWord1 = statusWord1;
        _StatusWord2 = statusWord2;
    }

    public StatusWords(ReadOnlySpan<byte> value)
    {
        if (value.Length < 2)
            throw new IccProtocolException(nameof(value));

        _StatusWord1 = value[0];
        _StatusWord2 = value[1];
    }

    #endregion

    #region Instance Members

    public string GetDescription() =>
        $"{nameof(StatusWord1)}: {GetStatusWord1Description()}; {nameof(StatusWord2)}: {GetStatusWord2Description()}";

    public StatusWord GetStatusWord1() => _StatusWord1;

    public string GetStatusWord1Description()
    {
        if (!StatusWord1.TryGet(_StatusWord1, out StatusWord1? result))
            return StatusWord1.Unknown.GetDescription();

        return result!.GetDescription();
    }

    public StatusWord GetStatusWord2() => _StatusWord2;

    public string GetStatusWord2Description()
    {
        if (!StatusWord2.TryGet(_StatusWord1, out StatusWord2? result))
            return StatusWord2.Unknown.GetDescription();

        return result!.GetDescription();
    }

    public StatusWordInfo GetStatusWordInfo()
    {
        if (!StatusWord1.TryGet(_StatusWord1, out StatusWord1? result))
            return StatusWordInfo.Unavailable;

        return result!.GetStatusWordInfo();
    }

    public override string ToString() => GetDescription();

    #endregion

    #region Equality

    public bool Equals(StatusWords? other)
    {
        if (other is null)
            return false;

        return (_StatusWord1 == other._StatusWord1) && (_StatusWord2 == other._StatusWord2);
    }

    public bool Equals(StatusWords? x, StatusWords? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override bool Equals(object? obj) => obj is StatusWords statusWords && Equals(statusWords);
    public int GetHashCode(StatusWords other) => other.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 8243;

        unchecked
        {
            int result = hash * _StatusWord1.GetHashCode();
            result += hash * _StatusWord2.GetHashCode();

            return result;
        }
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(StatusWords left, StatusWords right) => right.Equals(left);
    public static implicit operator ushort(StatusWords value) => (ushort) ((value._StatusWord1 << 8) | value._StatusWord2);
    public static bool operator !=(StatusWords left, StatusWords right) => !right.Equals(left);

    #endregion
}