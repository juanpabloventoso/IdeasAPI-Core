using IdeasAPI.Domain;

namespace IdeasAPI.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IdeasDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}