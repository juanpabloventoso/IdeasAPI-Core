using Microsoft.EntityFrameworkCore;

namespace IdeasAPI.Domain
{
    public class IdeasDbContext : DbContext
    {
        public virtual DbSet<Idea> Ideas { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public IdeasDbContext()
        {  
        }
		
        public IdeasDbContext(DbContextOptions<IdeasDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
