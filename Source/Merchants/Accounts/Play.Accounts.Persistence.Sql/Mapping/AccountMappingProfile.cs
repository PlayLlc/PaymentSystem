using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Persistence.Sql.Entities;

namespace Play.Accounts.Persistence.Sql.Mapping
{
    public class AccountMappingProfile : Profile
    {
        #region Constructor

        public AccountMappingProfile()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                // Address
                cfg.CreateMap<AddressDto, Address>().ForMember((dest) => dest, opt => opt.MapFrom(src => new Address(src)));
                cfg.CreateMap<Address, AddressDto>().ForMember((dest) => dest, opt => opt.MapFrom(src => src.AsDto()));

                // ContactInfo
                cfg.CreateMap<ContactInfoDto, Contact>().ForMember((dest) => dest, opt => opt.MapFrom(src => new Contact(src)));
                cfg.CreateMap<Contact, ContactInfoDto>().ForMember((dest) => dest, opt => opt.MapFrom(src => src.AsDto()));

                // PersonalInfo
                cfg.CreateMap<PersonalInfoDto, PersonalDetail>().ForMember((dest) => dest, opt => opt.MapFrom(src => new PersonalDetail(src)));
                cfg.CreateMap<PersonalDetail, PersonalInfoDto>().ForMember((dest) => dest, opt => opt.MapFrom(src => src.AsDto()));

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
                    .ForMember((dest) => dest, opt => opt.MapFrom(src => new User(src.Id, src.Address, src.Contact, src.PersonalDetail, src.IsActive)));
            });
        }

        #endregion
    }
}