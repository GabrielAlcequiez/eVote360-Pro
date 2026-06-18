# eVote360-Pro
## Autores
- Jose Antonio Rincon (2025-1426)
- Gabriel Enmanuel Alcequiez (2025-1062)

## Descripción del Proyecto
eVote360-Pro es una aplicación web MVC desarrollada para gestionar el ciclo completo de un proceso electoral electrónico. El sistema está estructurado utilizando **Arquitectura Onion** (Core.Domain, Core.Application, Infrastructure, Persistence y WebApp) y aplica patrones de diseño como Repository y Unit of Work.

## Tecnologías Usadas
- **Framework**: .NET 9.0 (ASP.NET Core MVC)
- **Lenguaje**: C# 13
- **ORM**: Entity Framework Core 9.0.3
- **Base de Datos**: SQL Server
- **Validación**: FluentValidation 12.1.1
- **Mapeo**: AutoMapper 16.1.1
- **Seguridad**: BCrypt.Net-Next, Cookie Authentication
- **Email**: MailKit (Gmail SMTP)
- **OCR**: Tesseract 5.2.0
- **Frontend**: HTML5, CSS3, Bootstrap, JavaScript (Razor Views)

## Requisitos Previos
Para poder compilar y ejecutar este proyecto, necesitas:
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) o superior.
- Una instancia de SQL Server (LocalDB o completa) en funcionamiento.

## Instrucciones para Correr el Proyecto

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/GabrielAlcequiez/eVote360-Pro.git
   cd eVote360-Pro
   ```

2. **Configurar la cadena de conexión**
   Abre el archivo `appsettings.json` dentro de la carpeta `eVote360_Pro.WebApp`. Asegúrate de que la cadena de conexión `DefaultConnection` apunte a tu servidor SQL Server:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=eVote360Pro;Trusted_Connection=True;TrustServerCertificate=true"
   }
   ```

3. **Aplicar las migraciones (Base de Datos)**
   Si no tienes la herramienta de comandos de EF Core instalada globalmente, instálala primero:
   ```bash
   dotnet tool install --global dotnet-ef
   ```
   Luego, aplica las migraciones a tu base de datos desde la raíz del proyecto:
   ```bash
   dotnet ef database update --project eVote360_Pro.Persistence --startup-project eVote360_Pro.WebApp
   ```

4. **Ejecutar la aplicación**
   Finalmente, inicia la aplicación ejecutando el siguiente comando:
   ```bash
   dotnet run --project eVote360_Pro.WebApp
   ```
   La consola mostrará la URL local en la que se está ejecutando el proyecto (por ejemplo, `http://localhost:5000` o `https://localhost:5001`). Abre esa URL en tu navegador web. Hay un `functional_document.md` en el cual te puedes guiar.
