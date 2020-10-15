using AutoMapper;
using Entities.Models;
using LuggageShare.DataTransferObjects;

namespace LuggageShare
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Owner, OwnerDto>();
            CreateMap<Account, AccountDto>();
            CreateMap<OwnerForCreationDto, Owner>();
            CreateMap<OwnerForUpdateDto, Owner>(); 
        }
    }
}
