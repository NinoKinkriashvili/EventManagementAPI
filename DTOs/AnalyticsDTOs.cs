namespace Pied_Piper.DTOs
{
    // Main analytics response
    public class AnalyticsDto
    {
        public KeyPerformanceIndicators KPIs { get; set; } = new();
        public EventStatusDistribution EventStatusDistribution { get; set; } = new(); // NEW
        public List<MonthlyRegistrationTrend> RegistrationTrend { get; set; } = new();
        public List<CategoryDistribution> CategoryDistribution { get; set; } = new();
        public List<TopEventByDemand> TopEvents { get; set; } = new();
        public List<DepartmentParticipation> DepartmentParticipation { get; set; } = new();
    }

    // Key Performance Indicators
    public class KeyPerformanceIndicators
    {
        public int TotalRegistrations { get; set; }
        public decimal RegistrationsChangePercentage { get; set; } // NEW: +12.5%, -5.2%, etc.

        public int ActiveParticipants { get; set; }
        public decimal ActiveParticipantsChangePercentage { get; set; } // NEW

        public decimal CancellationRate { get; set; }
        public decimal CancellationRateChange { get; set; } // NEW: Change in percentage points

        public int TotalEvents { get; set; }
        public decimal TotalEventsChangePercentage { get; set; } // NEW

        public int UpcomingEvents { get; set; }
        public int PastEvents { get; set; }
    }

    // NEW: Event Status Distribution
    public class EventStatusDistribution
    {
        public int AvailableEvents { get; set; }
        public int FullEvents { get; set; }
        public int WaitlistedEvents { get; set; }
        public decimal AvailablePercentage { get; set; }
        public decimal FullPercentage { get; set; }
        public decimal WaitlistedPercentage { get; set; }
    }

    // Monthly registration trend
    public class MonthlyRegistrationTrend
    {
        public string Month { get; set; } = string.Empty;
        public int Year { get; set; }
        public int CurrentMonthRegistrations { get; set; }
        public int PreviousMonthRegistrations { get; set; }
    }

    // Category distribution
    public class CategoryDistribution
    {
        public string CategoryName { get; set; } = string.Empty;
        public int EventCount { get; set; }
        public decimal Percentage { get; set; }
    }

    // Top events by demand (UPDATED)
    public class TopEventByDemand
    {
        public int Rank { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int CurrentCapacity { get; set; }
        public int MaxCapacity { get; set; }
        public int AvailableSlots { get; set; }
        public int WaitlistedCount { get; set; } // ADD THIS LINE
        public string EventStatus { get; set; } = string.Empty;
        public decimal UtilizationRate { get; set; }
    }

    // Department participation
    public class DepartmentParticipation
    {
        public string DepartmentName { get; set; } = string.Empty;
        public int TotalEmployees { get; set; }
        public int ActiveParticipants { get; set; }
        public decimal ParticipationRate { get; set; }
        public int TotalRegistrations { get; set; }
        public decimal AvgEventsPerEmployee { get; set; }
        public string Trend { get; set; } = string.Empty;
    }

    // Event status summary (UPDATED)
    public class EventStatusSummary
    {
        public int TotalEvents { get; set; }
        public int UpcomingEvents { get; set; }
        public int AvailableEvents { get; set; } // NEW
        public int FullEvents { get; set; } // NEW
        public int WaitlistedEvents { get; set; } // NEW
        public int CompletedEvents { get; set; }
    }

    // Registration status summary
    public class RegistrationStatusSummary
    {
        public int TotalRegistrations { get; set; }
        public int ConfirmedRegistrations { get; set; }
        public int WaitlistedRegistrations { get; set; }
        public int CancelledRegistrations { get; set; }
    }
}