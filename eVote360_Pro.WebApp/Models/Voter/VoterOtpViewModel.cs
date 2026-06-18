using System.ComponentModel.DataAnnotations;

namespace eVote360_Pro.WebApp.Models.Voter
{
    public class VoterOtpViewModel
    {
        [Required(ErrorMessage = "Debe ingresar el código de verificación enviado a su correo electrónico.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El código OTP debe tener exactamente 6 dígitos.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "El código OTP solo debe contener números.")]
        [Display(Name = "Código OTP de 6 dígitos")]
        public string OtpCode { get; set; } = string.Empty;
    }
}
