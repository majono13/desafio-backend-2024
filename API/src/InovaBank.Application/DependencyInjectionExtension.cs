using InovaBank.Application.Services.AutoMapper;
using InovaBank.Application.Services.Cryptography;
using InovaBank.Application.UseCases.User.Register;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using InovaBank.Application.UseCases.User.Login;

namespace InovaBank.Application
{
    public static class DependencyInjectionExtension
    {

        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddAutoMapper(services);
            AddUseCases(services);
            AddPasswordEncripter(services, configuration);
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        }

        private static void AddAutoMapper(IServiceCollection services)
        {

            services.AddScoped(opt =>
            {
                return new AutoMapper.MapperConfiguration(options =>
                {
                    options.AddProfile(new AutoMapping());
                }).CreateMapper();
            }); 
        }

        private static void AddPasswordEncripter(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(opt =>
            {
                return new PasswordEncripter();
            });
        }
    }
}
