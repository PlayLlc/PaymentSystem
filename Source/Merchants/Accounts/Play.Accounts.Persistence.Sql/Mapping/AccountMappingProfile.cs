using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using PlPlay.Accounts.Contractstos;
using PlayPlay.Accounts.Contracts.Dtos
using PPlay.Accounts.Domain.Aggregatesusing PlaPlay.Accounts.Domain.Entitiestities;
Play.Accounts.Persistence.Sql.EntitiesSql.Mapping
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
                cfg.CreateMap<ContactInfoDto, ContactInfo>().ForMember((dest) => dest, opt => opt.MapFrom(src => new ContactInfo(src)));
                cfg.CreateMap<ContactInfo, ContactInfoDto>().ForMember((dest) => dest, opt => opt.MapFrom(src => src.AsDto()));

                // PersonalInfo
                cfg.CreateMap<PersonalInfoDto, PersonalInfo>().ForMember((dest) => dest, opt => opt.MapFrom(src => new PersonalInfo(src)));
                cfg.CreateMap<PersonalInfo, PersonalInfoDto>().ForMember((dest) => dest, opt => opt.MapFrom(src => src.AsDto()));

                // Roles
                cfg.CreateMap<UserRole, RoleIdentity>()
                .ForMember((dest) => de
                From(src => new RoleIdentity
                {
                    
                 
                    src.Name
  
                                  c
                dentity, UserRole>().ForMember((dest) => dest, opt => opt.MapFrom(src => new UserRole(src.Id, src.Name)));

                // User
                cfg.CreateMap<User, UserIdentity>().ForMember((dest) => dest, opt => opt.MapFrom(src => new UserIdentity(src.AsDto())));
                cfg.CreateMap<UserIdentity, User>()
                    .ForMember((dest) =
                    From(src => new User(src.Id, src.Address, src.ContactInfo, src.PersonalInfo, src.IsActive)));
            });
        }

        #endregion
    }
}