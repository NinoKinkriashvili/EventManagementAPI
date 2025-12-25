namespace Pied_Piper.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int? EventId { get; set; }
        public string? EventTitle { get; set; }
        public bool IsSeen { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class NotificationListResponse
    {
        public List<NotificationDto> New { get; set; } = new();
        public List<NotificationDto> Earlier { get; set; } = new();
        public int TotalUnseenCount { get; set; }
    }
}