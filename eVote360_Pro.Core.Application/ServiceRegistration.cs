using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Application.Services;

namespace eVote360_Pro.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            // Registrar AutoMapper escaneando este ensamblado para perfiles de mapeo
            services.AddAutoMapper(cfg => 
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            // Registrar los servicios de la capa de aplicación
            services.AddTransient<IUserService, UserService>();
        }
    }
}
