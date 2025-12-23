using luckyoneApiv3.Entity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace luckyoneApiv3.Data
{
    public class ApplicationDbContext : DbContext
    {
      
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }


        public  DbSet<Users> User {  get; set; }
        public DbSet<Contests> Contests { get; set; }
        public DbSet<ContestParticipants> ContestParticipants { get; set; }
        public DbSet<PointsTransactions> PointTransactions { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Winners> Winners { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }

        public DbSet<Notifications> Notifications { get; set; }




    }
}
