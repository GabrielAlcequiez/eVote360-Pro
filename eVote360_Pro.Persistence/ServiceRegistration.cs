using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Persistence.Context;
using eVote360_Pro.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eVote360_Pro.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                m => m.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
            #endregion

            #region Repositories
            // Registra el repositorio base genérico
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            
            // Registra el repositorio específico de usuarios
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IElectedOfficeRepository, ElectedOfficeRepository>();
            services.AddTransient<IElectionRepository, ElectionRepository>();
            services.AddTransient<ICitizenRepository, CitizenRepository>();
            services.AddTransient<IPoliticalPartyRepository, PoliticalPartyRepository>();
            services.AddTransient<IPartyLeaderRepository, PartyLeaderRepository>();
            services.AddTransient<IPoliticalAllianceRepository, PoliticalAllianceRepository>();
            services.AddTransient<IVerificationCodeRepository, VerificationCodeRepository>();
            
            // Registra la Unidad de Trabajo minimalista
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            #endregion
        }
    }
}