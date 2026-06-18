namespace eVote360_Pro.Core.Domain.Entities
{
    public class VerificationCode
    {
        public Guid Id { get; private set; }

        public Guid CitizenId { get; private set; }
        public Citizen Citizen { get; private set; } = null!;

        public Guid ElectionId { get; private set; }
        public Election Election { get; private set; } = null!;

        public string Code { get; private set; } = null!;

        public DateTime GeneratedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsUsed { get; private set; }

        protected VerificationCode() { }

       
        public VerificationCode(Guid citizenId, Guid electionId, string code)
        {
            Id = Guid.NewGuid();
            CitizenId = citizenId;
            ElectionId = electionId;
            Code = code;
            GeneratedAt = DateTime.UtcNow;
            ExpiresAt = GeneratedAt.AddMinutes(5); 
            IsUsed = false;
        }

        public void Use()
        {
            if (IsUsed)
                throw new InvalidOperationException("Este código de verificación ya fue utilizado.");

            if (DateTime.UtcNow > ExpiresAt)
                throw new InvalidOperationException("El código de verificación ha expirado. Solicite un nuevo código para continuar.");

            IsUsed = true;
        }
    }
}