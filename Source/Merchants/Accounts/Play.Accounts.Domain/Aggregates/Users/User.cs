﻿using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

public class User : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;

    // private readonly MerchantId _MerchantId;
    private readonly Address _Address;
    private readonly ContactInfo _ContactInfo;
    private readonly PersonalInfo _PersonalInfo;
    private readonly bool _IsActive;

    #endregion

    #region Constructor

    private User()
    {
        // Entity Framework only
    }

    public User(string id, Address address, ContactInfo contactInfo, PersonalInfo personalInfo, bool isActive, params UserRole[] roles)
    {
        _Id = id;

        //_MerchantId = merchantId;
        _Address = address;
        _ContactInfo = contactInfo;
        _PersonalInfo = personalInfo;
        _IsActive = isActive;
    }

    #endregion

    #region Instance Members

    public static User CreateFromUserRegistration(
        string userRegistrationId, Address address, ContactInfo contactInfo, PersonalInfo personalInfo, params UserRole[] roles)
    {
        User user = new User(userRegistrationId, address, contactInfo, personalInfo, true, roles);
        user.Publish(new UserCreated(user.GetId()));

        return user;
    }

    //public void AddRole(UserRole role)
    //{
    //    if (_Roles.Add(role))
    //        Publish(new UserRoleAdded(_Id!, role));
    //}

    public override string GetId()
    {
        return _Id;
    }

    public override UserDto AsDto()
    {
        return new UserDto
        {
            Id = _Id, /* MerchantId = _MerchantId.Id,*/
            AddressDto = _Address.AsDto(),
            ContactInfoDto = _ContactInfo.AsDto(),
            PersonalInfoDto = _PersonalInfo.AsDto(),
            IsActive = _IsActive
        };
    }

    #endregion
}