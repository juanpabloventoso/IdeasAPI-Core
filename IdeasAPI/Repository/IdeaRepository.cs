using IdeasAPI.Domain;

namespace IdeasAPI.Repository
{
    public class IdeaRepository : GenericRepository<Idea>, IIdeaRepository
    {
        public IdeaRepository(IdeasDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}