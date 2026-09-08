
using Microsoft.EntityFrameworkCore;
using RESTBottle.Models;
namespace RESTBottle.Service
{
    public class BottlesDBContext : DbContext
    {
        public BottlesDBContext(DbContextOptions<BottlesDBContext> options): base(options) 
        {
            
        }

        public DbSet<Bottle> Bottles { get; set; }
    }
}
