using AutoMapper;

using Play.Domain.Common.Dtos;
using Play.Domain.Common.Entities;
using Play.Identity.Contracts.Dtos;
using Play.Identity.Domain.Aggregates;
using Play.Identity.Domain.Entities;
using Play.Identity.Persistence.Sql.Entities;

namespace Play.Identity.Persistence.Sql.Mapping;

public class AccountMappingProfile : Profile
{
    #region Constructor

    public AccountMappingProfile()
    {
        // Address
        CreateMap<AddressDto, Address>().ConstructUsing(a => new(a));
        CreateMap<Address, AddressDto>().ConstructUsing(a => a.AsDto());

        // BusinessInfo
        CreateMap<BusinessInfoDto, BusinessInfo>().ConstructUsing(a => new(a));
        CreateMap<BusinessInfo, BusinessInfoDto>().ConstructUsing(a => a.AsDto());

        // ConfirmationCode
        CreateMap<ConfirmationCodeDto, ConfirmationCode>().ConstructUsing(a => new(a));
        CreateMap<ConfirmationCode, ConfirmationCodeDto>().ConstructUsing(a => a.AsDto());

        // Contact
        CreateMap<ContactDto, Contact>().ConstructUsing(a => new(a));
        CreateMap<Contact, ContactDto>().ConstructUsing(a => a.AsDto());

        // Password
        CreateMap<PasswordDto, Password>().ConstructUsing(a => new(a));
        CreateMap<Password, PasswordDto>().ConstructUsing(a => a.AsDto());

        // PersonalDetail
        CreateMap<PersonalDetailDto, PersonalDetail>().ConstructUsing(a => new(a));
        CreateMap<PersonalDetail, PersonalDetailDto>().ConstructUsing(a => a.AsDto());

        // UserRole
        CreateMap<UserRoleDto, UserRole>().ConstructUsing(a => new(a));
        CreateMap<UserRole, UserRoleDto>().ConstructUsing(a => a.AsDto());

        // Roles
        CreateMap<UserRole, RoleIdentity>().ConstructUsing(a => new(a.Name));
        CreateMap<RoleIdentity, UserRole>().ConstructUsing(a => new(a.Name));

        // User
        CreateMap<User, UserIdentity>().ConstructUsing(a => new(a.AsDto()));
        CreateMap<UserIdentity, User>().ConstructUsing(a => new(a.AsDto()));
    }

    #endregion
}