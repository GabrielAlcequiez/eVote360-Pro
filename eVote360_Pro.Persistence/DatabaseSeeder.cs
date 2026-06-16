using System;
using System.Linq;
using System.Threading.Tasks;
using eVote360_Pro.Core.Application.Helpers;
using eVote360_Pro.Core.Domain.Common.Enums;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

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

            // Sembrar una elección activa para pruebas si no existe ninguna
        
            // if (!await context.Elections.AnyAsync())
            // {
            //     var testElection = new Election("Elecciones Presidenciales de Prueba", DateTime.Today.AddDays(7));
            //     testElection.Activate(); // Cambiar estado a activa
            //     await context.Elections.AddAsync(testElection);
            //     await context.SaveChangesAsync();
            // }

            // Sembrar un ciudadano de prueba asociado al correo configurado para recibir el OTP
            if (!await context.Citizens.AnyAsync())
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var testEmail = configuration["EmailSettings:SenderEmail"] ?? "test@gmail.com";
                
                // Formato limpio de 11 dígitos para la cédula (00100000000)
                var testCitizen = new Citizen("Juan", "Pérez", testEmail, "00100000000");
                await context.Citizens.AddAsync(testCitizen);
                await context.SaveChangesAsync();
            }
        }
    }
}
