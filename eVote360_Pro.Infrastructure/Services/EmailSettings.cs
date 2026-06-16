namespace eVote360_Pro.Infrastructure.Services
{
    /// <summary>
    /// Configuración SMTP leída desde appsettings.json → sección "EmailSettings".
    /// Se enlaza mediante IOptions&lt;EmailSettings&gt; en la DI.
    /// </summary>
    public class EmailSettings
    {
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 587;
        public string SmtpUser { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;

        // false: sin TLS (MailHog, Papercut, etc.)
        // true: StartTls (Gmail, Outlook, etc.)
        public bool UseSsl { get; set; } = true;

        // false: sin autenticación (servidores locales de prueba)
        // true: autenticar con SmtpUser/SmtpPassword
        public bool RequiresAuthentication { get; set; } = true;
    }
}
