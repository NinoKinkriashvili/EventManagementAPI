using System.ComponentModel.DataAnnotations;

namespace Pied_Piper.DTOs.User
{
    public class CheckOtpRequest
    {
        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be exactly 6 digits")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must contain only digits")]
        public string Otp { get; set; } = string.Empty;
    }
}