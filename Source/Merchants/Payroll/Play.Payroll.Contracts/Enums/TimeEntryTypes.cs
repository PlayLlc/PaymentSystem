using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Core;

namespace Play.Payroll.Contracts.Enums;

public record TimeEntryTypes : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, TimeEntryTypes> _ValueObjectMap;
    public static readonly TimeEntryTypes Empty;
    public static readonly TimeEntryTypes PaidTime;
    public static readonly TimeEntryTypes UnpaidTime;

    #endregion

    #region Constructor

    private TimeEntryTypes(string value) : base(value)
    { }

    static TimeEntryTypes()
    {
        Empty = new TimeEntryTypes("");
        PaidTime = new TimeEntryTypes(nameof(PaidTime));
        UnpaidTime = new TimeEntryTypes(nameof(UnpaidTime));

        _ValueObjectMap = new Dictionary<string, TimeEntryTypes>
        {
            {PaidTime, PaidTime},
            {UnpaidTime, UnpaidTime}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override EnumObjectString[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(string value, out EnumObjectString? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out TimeEntryTypes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}