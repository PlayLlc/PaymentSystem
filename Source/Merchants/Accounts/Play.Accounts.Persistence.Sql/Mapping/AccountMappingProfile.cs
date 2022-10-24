using AutoMapper;

using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Persistence.Sql.Entities;

namespace Play.Accounts.Persistence.Sql.Mapping;

public class AccountMappingProfile : Profile
{
    #region Constructor

    public AccountMappingProfile()
    {
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            // Address
            cfg.CreateMap<AddressDto, Address>().ConstructUsing(a => new Address(a));
            cfg.CreateMap<Address, AddressDto>().ConstructUsing(a => a.AsDto());

            // BusinessInfo
            cfg.CreateMap<BusinessInfoDto, BusinessInfo>().ConstructUsing(a => new BusinessInfo(a));
            cfg.CreateMap<BusinessInfo, BusinessInfoDto>().ConstructUsing(a => a.AsDto());

            // ConfirmationCode
            cfg.CreateMap<ConfirmationCodeDto, ConfirmationCode>().ConstructUsing(a => new ConfirmationCode(a));
            cfg.CreateMap<ConfirmationCode, ConfirmationCodeDto>().ConstructUsing(a => a.AsDto());

            // Contact
            cfg.CreateMap<ContactDto, Contact>().ConstructUsing(a => new Contact(a));
            cfg.CreateMap<Contact, ContactDto>().ConstructUsing(a => a.AsDto());

            // Password
            cfg.CreateMap<PasswordDto, Password>().ConstructUsing(a => new Password(a));
            cfg.CreateMap<Password, PasswordDto>().ConstructUsing(a => a.AsDto());

            // PersonalDetail
            cfg.CreateMap<PersonalDetailDto, PersonalDetail>().ConstructUsing(a => new PersonalDetail(a));
            cfg.CreateMap<PersonalDetail, PersonalDetailDto>().ConstructUsing(a => a.AsDto());

            // UserRole
            cfg.CreateMap<UserRoleDto, UserRole>().ConstructUsing(a => new UserRole(a));
            cfg.CreateMap<UserRole, UserRoleDto>().ConstructUsing(a => a.AsDto());

            // Roles
            cfg.CreateMap<UserRole, RoleIdentity>().ConstructUsing(a => new RoleIdentity(a.Name));
            cfg.CreateMap<RoleIdentity, UserRole>().ConstructUsing(a => new UserRole(a.Name));

            // User
            cfg.CreateMap<User, UserIdentity>().ConstructUsing(a => new UserIdentity(a.AsDto()));
            cfg.CreateMap<UserIdentity, User>().ConstructUsing(a => new User(a.AsDto()));
        });
    }

    #endregion
}