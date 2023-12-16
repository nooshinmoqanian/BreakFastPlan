using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DBContexts
{

    public class BreakfastContext : DbContext
    {
        public DbSet<Breakfast> Breakfasts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Users> User { get; set; }
        public DbSet<BreakfastTag> BreakfastTags { get; set; }

        public BreakfastContext(DbContextOptions<BreakfastContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BreakfastTag>()
                .HasKey(bt => new { bt.BreakfastId, bt.TagId }); 

            modelBuilder.Entity<BreakfastTag>()
                .HasOne(bt => bt.Breakfast)
                .WithMany(b => b.BreakfastTags)
                .HasForeignKey(bt => bt.BreakfastId);

            modelBuilder.Entity<BreakfastTag>()
                .HasOne(bt => bt.Tag)
                .WithMany(t => t.BreakfastTags)
                .HasForeignKey(bt => bt.TagId);
        }
    }


}
