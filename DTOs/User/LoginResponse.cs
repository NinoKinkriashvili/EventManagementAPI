namespace Pied_Piper.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } // NEW
        public string Department { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}