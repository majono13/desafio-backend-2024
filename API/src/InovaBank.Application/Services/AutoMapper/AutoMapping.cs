using AutoMapper;
using InovaBank.Communication.Requests;
using InovaBank.Domain.Entities;

namespace InovaBank.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
        }

        private void RequestToDomain()
        {
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Password, opt => opt.Ignore()); //Ignoar senha
        }
    }
}
