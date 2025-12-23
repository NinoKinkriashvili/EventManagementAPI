namespace Pied_Piper.DTOs
{
    public class RegistrationDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
    }
}