using System;

namespace Play.Globalization.Time;

/// <summary>
///     This struct represents a <see cref="TimeSpan" /> that is initialized with Milliseconds
/// </summary>
public readonly record struct Seconds
{
    #region Static Metadata

    public static readonly Seconds Zero = new(0);

    #endregion

    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    public Seconds(Milliseconds value)
    {
        _Value = (uint) value / 1000;
    }

    #endregion

    #region Instance Members

    public TimeSpan AsTimeSpan() => GetTimeSpan(_Value);
    private static TimeSpan GetTimeSpan(uint value) => new(GetDays(value), GetHours(value), GetMinutes(value), GetSeconds(value));
    private static int GetDays(uint value) => GetHours(value) / 24;
    private static int GetHours(uint value) => GetMinutes(value) / 60;
    private static int GetMinutes(uint value) => checked((int) value / 60);
    private static int GetSeconds(uint value) => (int) value % 60;

    #endregion

    #region Operator Overrides

    public static explicit operator uint(Seconds value) => value._Value;
    public static implicit operator TimeSpan(Seconds value) => value.AsTimeSpan();
    public static implicit operator Seconds(Milliseconds value) => new(value);

    #endregion
}