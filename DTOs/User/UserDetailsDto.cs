namespace Pied_Piper.DTOs
{
    public class UserDetailsDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalRegistrations { get; set; }
        public int TotalEventsCreated { get; set; }
    }
}