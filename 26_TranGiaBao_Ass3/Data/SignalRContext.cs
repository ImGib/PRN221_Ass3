using _26_TranGiaBao_Ass3.Models;
using Microsoft.EntityFrameworkCore;

namespace _26_TranGiaBao_Ass3.Data
{
    public class SignalRContext:DbContext
    {
        public DbSet<PostCategories> PostCategories { get; set; }
        public DbSet<AppUsers> AppUsers { get; set; }
        public DbSet<Posts> Posts { get; set; }

        public SignalRContext(DbContextOptions<SignalRContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Posts>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryID);

            modelBuilder.Entity<Posts>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.AuthorID);
        }
    }
}
