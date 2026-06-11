using System;
using System.Linq;
using System.Threading.Tasks;
using eVote360_Pro.Core.Application.Helpers;
using eVote360_Pro.Core.Domain.Common.Enums;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eVote360_Pro.Persistence
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            // Aplicar migraciones pendientes de forma automática
            await context.Database.MigrateAsync();

            // Verificar si existen usuarios creados en la base de datos
            if (!await context.Users.AnyAsync())
            {
                // Hashear la contraseña por defecto
                var hashedPassword = PasswordHelper.HashPassword("Admin12345");

                var defaultAdmin = new User(
                    "Admin",
                    "Electoral",
                    "admin@evote360.com",
                    "admin",
                    hashedPassword,
                    Role.Administrator
                );

                var randomCandidate = new User(
                    "Juan",
                    "Papaleta",
                    "jose@gmail.com",
                    "papaleta123",
                    "holaklk12",
                    Role.PoliticalLeader
                );

                await context.Users.AddAsync(defaultAdmin);
                await context.Users.AddAsync(randomCandidate);
                await context.SaveChangesAsync();
            }
        }
    }
}
