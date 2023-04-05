﻿using System.Collections.Immutable;

using Play.Core;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Domain.Enums;

public record UserRegistrationStatuses : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, UserRegistrationStatuses> _ValueObjectMap;
    public static UserRegistrationStatuses Empty;
    public static UserRegistrationStatuses WaitingForRiskAnalysis;
    public static UserRegistrationStatuses WaitingForEmailVerification;
    public static UserRegistrationStatuses WaitingForSmsVerification;
    public static UserRegistrationStatuses Approved;
    public static UserRegistrationStatuses Expired;
    public static UserRegistrationStatuses Rejected;

    #endregion

    #region Constructor

    private UserRegistrationStatuses(string value) : base(value)
    { }

    static UserRegistrationStatuses()
    {
        Empty = new("");
        WaitingForRiskAnalysis = new(nameof(WaitingForRiskAnalysis));
        WaitingForEmailVerification = new(nameof(WaitingForEmailVerification));
        WaitingForSmsVerification = new(nameof(WaitingForSmsVerification));
        Approved = new(nameof(Approved));
        Expired = new(nameof(Expired));
        Rejected = new(nameof(Rejected));

        _ValueObjectMap = new Dictionary<string, UserRegistrationStatuses>
        {
            {WaitingForRiskAnalysis, WaitingForRiskAnalysis},
            {WaitingForEmailVerification, WaitingForEmailVerification},
            {WaitingForSmsVerification, WaitingForSmsVerification},
            {Approved, Approved},
            {Expired, Expired},
            {Rejected, Rejected}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override UserRegistrationStatuses[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(string value, out EnumObjectString? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out UserRegistrationStatuses? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator UserRegistrationStatus(UserRegistrationStatuses value) => new(value);

    #endregion
}