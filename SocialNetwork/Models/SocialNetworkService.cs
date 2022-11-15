using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;

namespace SocialNetwork.Models
{
    public class SocialNetworkService
    {
        private readonly SocialNetworkContext _context;

        public SocialNetworkService(SocialNetworkContext context)
        {
            //// constructor called every time when new view is returned?
            _context = context;
        }

        public async Task<ICollection<User>> GetAllUsers() => await _context.User.ToListAsync();

        public async Task<User?> GetUserByLoginOrNull(string login) => await _context.User.FindAsync(login);

        public void CreateUser(User user)
        {
            _context.Add(user);
            _context.SaveChangesAsync();
        }

        public void CreateUsers(ICollection<User> users)
        {
            _context.User.AddRange(users);
            _context.SaveChangesAsync();
        }

        public void RemoveUser(User userToRemove)
        {
            _context.User.Remove(userToRemove);
            _context.SaveChangesAsync();
        }

        public async void RemoveAllExceptAdmin()
        {
            _context.User.RemoveRange(await _context.User.Where(u=>u.Login!="admin").ToListAsync());
        }
    }
}
