namespace Pied_Piper.DTOs
{
    public class AgendaItemDto
    {
        public int? Id { get; set; } // Nullable for create requests
        public string Time { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}