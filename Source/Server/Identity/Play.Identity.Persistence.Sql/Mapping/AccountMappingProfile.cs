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
        CreateMap<AddressDto, Address>().ConstructUsing(a => new Address(a));
        CreateMap<Address, AddressDto>().ConstructUsing(a => a.AsDto());

        // BusinessInfo
        CreateMap<BusinessInfoDto, BusinessInfo>().ConstructUsing(a => new BusinessInfo(a));
        CreateMap<BusinessInfo, BusinessInfoDto>().ConstructUsing(a => a.AsDto());

        // ConfirmationCode
        CreateMap<ConfirmationCodeDto, EmailConfirmationCode>().ConstructUsing(a => new EmailConfirmationCode(a));
        CreateMap<EmailConfirmationCode, ConfirmationCodeDto>().ConstructUsing(a => a.AsDto());

        CreateMap<ConfirmationCodeDto, SmsConfirmationCode>().ConstructUsing(a => new SmsConfirmationCode(a));
        CreateMap<SmsConfirmationCode, ConfirmationCodeDto>().ConstructUsing(a => a.AsDto());

        // Contact
        CreateMap<ContactDto, Contact>().ConstructUsing(a => new Contact(a));
        CreateMap<Contact, ContactDto>().ConstructUsing(a => a.AsDto());

        // Password
        CreateMap<PasswordDto, Password>().ConstructUsing(a => new Password(a));
        CreateMap<Password, PasswordDto>().ConstructUsing(a => a.AsDto());

        // PersonalDetail
        CreateMap<PersonalDetailDto, PersonalDetail>().ConstructUsing(a => new PersonalDetail(a));
        CreateMap<PersonalDetail, PersonalDetailDto>().ConstructUsing(a => a.AsDto());

        // UserRole
        CreateMap<UserRoleDto, UserRole>().ConstructUsing(a => new UserRole(a));
        CreateMap<UserRole, UserRoleDto>().ConstructUsing(a => a.AsDto());

        // Roles
        CreateMap<UserRole, RoleIdentity>().ConstructUsing(a => new RoleIdentity(a.Name));
        CreateMap<RoleIdentity, UserRole>().ConstructUsing(a => new UserRole(a.Name));

        // User
        CreateMap<User, UserIdentity>().ConstructUsing(a => new UserIdentity(a.AsDto()));
        CreateMap<UserIdentity, User>().ConstructUsing(a => new User(a.AsDto()));
    }

    #endregion
}