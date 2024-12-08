using AutoMapper;
using InovaBank.Communication.Requests.Account;
using InovaBank.Communication.Requests.User;
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
                .ForMember(dest => dest.Password, opt => opt.Ignore()); //Ignoar senha

            CreateMap<RequesteRegisterAccountJson, Domain.Entities.Account>();
        }
    }
}
