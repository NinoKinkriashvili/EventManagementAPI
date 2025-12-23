using Pied_Piper.Models;

namespace Pied_Piper.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task<Event?> GetByIdWithDetailsAsync(int id);
        Task<Event> CreateAsync(Event ev);
        Task UpdateAsync(Event ev);
        Task DeleteAsync(int id); // soft delete
        Task<IEnumerable<Event>> GetUpcomingAsync();
        Task<IEnumerable<EventType>> GetEventTypesAsync();
        Task<IEnumerable<Category>> GetCategoriesAsync(); // NEW
        Task<EventType?> GetEventTypeByNameAsync(string name);
        Task<Category?> GetCategoryByTitleAsync(string title);
        Task<IEnumerable<Event>> GetAllIncludingHiddenAsync();
    }
}