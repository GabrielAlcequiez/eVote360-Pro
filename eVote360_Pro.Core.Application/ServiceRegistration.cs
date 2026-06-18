using Microsoft.Extensions.DependencyInjection;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Application.Services;

namespace eVote360_Pro.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            // Registrar los servicios de la capa de aplicación
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IElectedOfficeService, ElectedOfficeService>();
            services.AddTransient<ICitizenService, CitizenService>();
            services.AddTransient<IPoliticalPartyService, PoliticalPartyService>();
            services.AddTransient<IPartyLeaderService, PartyLeaderService>();
            services.AddTransient<ICandidateService, CandidateService>();
            services.AddTransient<IPoliticalAllianceService, PoliticalAllianceService>();
            services.AddTransient<ICandidateOfficeAssignmentService, CandidateOfficeAssignmentService>();

            services.AddTransient<IElectionService, ElectionService>();
            services.AddTransient<IBallotService, BallotService>();
            services.AddTransient<IVoterAuthService, VoterAuthService>();
            services.AddTransient<IDashboardService, DashboardService>();

            services.AddTransient<IElectionStatusService, ElectionStatusService>();
        }
    }
}
