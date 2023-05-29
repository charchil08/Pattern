using Microsoft.EntityFrameworkCore;
using Pattern.Entity.Data;
using Pattern.Entity.DataModels;
using Pattern.Repository.Interface;

namespace Pattern.Repository
{
    public class AccountRepo: GenericRepo<User>, IAccountRepo
    {

        private readonly PatternContext _context;
        private readonly DbSet<User> _dbSet;

        public AccountRepo(PatternContext context) : base(context)
        {
            _context = context;
            _dbSet= _context.Set<User>();
        }
        
        public async Task<User?> LoginAsync(string email, string password)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

    }
}
