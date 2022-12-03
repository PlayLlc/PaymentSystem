using System.Collections.Immutable;

using Play.Core;
using Play.TimeClock.Domain.ValueObject;

namespace Play.TimeClock.Domain.Enums;

public record TimeClockStatuses : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, TimeClockStatuses> _ValueObjectMap;
    public static readonly TimeClockStatuses Empty;
    public static readonly TimeClockStatuses ClockedIn;
    public static readonly TimeClockStatuses ClockedOut;

    #endregion

    #region Constructor

    private TimeClockStatuses(string value) : base(value)
    { }

    static TimeClockStatuses()
    {
        Empty = new TimeClockStatuses("");
        ClockedIn = new TimeClockStatuses(nameof(ClockedIn));
        ClockedOut = new TimeClockStatuses(nameof(ClockedOut));

        _ValueObjectMap = new Dictionary<string, TimeClockStatuses>
        {
            {ClockedIn, ClockedIn},
            {ClockedOut, ClockedOut}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override TimeClockStatuses[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(string value, out EnumObjectString? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out TimeClockStatuses? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator TimeClockStatus(TimeClockStatuses value) => new TimeClockStatus(value);

    #endregion
}