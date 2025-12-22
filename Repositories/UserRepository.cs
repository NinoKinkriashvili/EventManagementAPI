using Pied_Piper.Data;
using Pied_Piper.Models;
using Microsoft.EntityFrameworkCore;

namespace Pied_Piper.Repositories
{
     public class UserRepository : IUserRepository
     {
          public Task<User?> GetByIdAsync(int id)
          {
               throw new NotImplementedException();
          }

          public Task<User?> GetByEmailAsync(string email)
          {
               throw new NotImplementedException();
          }

          public Task<IEnumerable<User>> GetAllAsync()
          {
               throw new NotImplementedException();
          }

          public Task<User> CreateAsync(User user)
          {
               throw new NotImplementedException();
          }

          public Task UpdateAsync(User user)
          {
               throw new NotImplementedException();
          }

          public Task DeactivateAsync(int id)
          {
               throw new NotImplementedException();
          }
     }
}
