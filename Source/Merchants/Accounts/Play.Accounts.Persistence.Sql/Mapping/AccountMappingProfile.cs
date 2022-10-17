using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Play.Accounts.Contracts.Common;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates.Users;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Persistence.Sql.Entities;

namespace Play.Accounts.Persistence.Sql.Mapping
{
    public class AccountMappingProfile : Profile
    {
        #region Constructor

        public AccountMappingProfile()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Address
                cfg.CreateMap<AddressDto, Address>().ForMember((dest) => dest, opt => opt.MapFrom(src => new Address(src)));
                cfg.CreateMap<Address, AddressDto>().ForMember((dest) => dest, opt => opt.MapFrom(src => src.AsDto()));

                // ContactInfo
                cfg.CreateMap<ContactInfoDto, ContactInfo>().ForMember((dest) => dest, opt => opt.MapFrom(src => new ContactInfo(src)));
                cfg.CreateMap<ContactInfo, ContactInfoDto>().ForMember((dest) => dest, opt => opt.MapFrom(src => src.AsDto()));

                // PersonalInfo
                cfg.CreateMap<PersonalInfoDto, PersonalInfo>().ForMember((dest) => dest, opt => opt.MapFrom(src => new PersonalInfo(src)));
                cfg.CreateMap<PersonalInfo, PersonalInfoDto>().ForMember((dest) => dest, opt => opt.MapFrom(src => src.AsDto()));

                // Roles
                cfg.CreateMap<UserRole, RoleIdentity>()
                .ForMember((dest) => dest, opt => opt.MapFrom(src => new RoleIdentity
                {
                    Id = src.Id,
                    Name = src.Name
                }));
                cfg.CreateMap<RoleIdentity, UserRole>().ForMember((dest) => dest, opt => opt.MapFrom(src => new UserRole(src.Id, src.Name)));

                // User
                cfg.CreateMap<User, UserIdentity>().ForMember((dest) => dest, opt => opt.MapFrom(src => new UserIdentity(src.AsDto())));
                cfg.CreateMap<UserIdentity, User>()
                    .ForMember((dest) => dest, opt => opt.MapFrom(src => new User(src.Id, src.Address, src.ContactInfo, src.PersonalInfo, src.IsActive)));
            });
        }

        #endregion
    }
}