using Core.Entities;
using Microsoft.EntityFrameworkCore;
//78242216
namespace Infrastructure
{
    public class IQADbContext: DbContext
    {
        public IQADbContext(DbContextOptions<IQADbContext> options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}