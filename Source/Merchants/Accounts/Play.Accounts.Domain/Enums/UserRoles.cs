﻿using Play.Core;

using System.Collections.Immutable;

using Play.Accounts.Domain.Entities;

namespace Play.Accounts.Domain.Enums;

public record UserRoles : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, UserRoles> _ValueObjectMap;
    public static readonly UserRoles Empty;
    public static readonly UserRoles SuperAdmin;
    public static readonly UserRoles Administrator;
    public static readonly UserRoles SalesAssociate;

    #endregion

    #region Instance Values

    public readonly string Name;

    #endregion

    #region Constructor

    private UserRoles(string value) : base(value)
    {
        Name = value;
    }

    static UserRoles()
    {
        Empty = new UserRoles("");
        SuperAdmin = new UserRoles(nameof(SuperAdmin));
        Administrator = new UserRoles(nameof(Administrator));
        SalesAssociate = new UserRoles(nameof(SalesAssociate));
        _ValueObjectMap = new Dictionary<string, UserRoles>
        {
            {SuperAdmin, SuperAdmin},
            {Administrator, Administrator},
            {SalesAssociate, SalesAssociate}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override UserRoles[] GetAll()
    {
        return _ValueObjectMap.Values.ToArray();
    }

    public UserRoles Get(string value)
    {
        return _ValueObjectMap[value];
    }

    public override bool TryGet(string value, out EnumObjectString? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out UserRoles? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator UserRole(UserRoles value)
    {
        return new UserRole(value);
    }

    #endregion
}