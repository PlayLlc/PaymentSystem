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
                cfg.CreateMap<AddressDto, Address>().ForMember((dest) => dest, opt => opt.MapFrom(src => new Address(src)));
                cfg.CreateMap<Address, AddressDto>().ForMember((dest) => dest, opt => opt.MapFrom(src => src.AsDto()));

                cfg.CreateMap<ContactInfoDto, ContactInfo>().ForMember((dest) => dest, opt => opt.MapFrom(src => new ContactInfo(src)));
                cfg.CreateMap<ContactInfo, ContactInfoDto>().ForMember((dest) => dest, opt => opt.MapFrom(src => src.AsDto()));

                cfg.CreateMap<PersonalInfoDto, PersonalInfo>().ForMember((dest) => dest, opt => opt.MapFrom(src => new PersonalInfo(src)));
                cfg.CreateMap<PersonalInfo, PersonalInfoDto>().ForMember((dest) => dest, opt => opt.MapFrom(src => src.AsDto()));
            });
        }

        #endregion
    }
}