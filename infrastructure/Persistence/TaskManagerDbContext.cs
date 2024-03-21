using Microsoft.EntityFrameworkCore;
using Domain.Entities;
namespace infrastructure.Persistence
{
    public class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> Options) : base(Options)
        {
        }

        public DbSet<Tasks> tasks { get; set; }
        public DbSet<Projects> projects { get; set; }
        public DbSet<Employees> employees { get; set; }

        public DbSet<Teams> teams { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Activities> Activities { get; set; }
        public DbSet<Invites> Invites { get; set; }
        public DbSet<ResetPassword> resetPassword { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employees>().HasMany(e => e.Tasks).WithMany(t => t.Asignees);
            modelBuilder.Entity<Employees>().HasMany(e => e.tasksReported).WithOne(t => t.Reporter);
            modelBuilder.Entity<Employees>().HasMany(e => e.activities).WithOne(t => t.actor);
            modelBuilder.Entity<Tasks>().HasMany(e=>e.Comments).WithOne(c=>c.MessageTask).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Tasks>().HasMany(e => e.Attachments).WithOne(c => c.task).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Tasks>().HasMany(e => e.Activities).WithOne(c => c.task).OnDelete(DeleteBehavior.Cascade);

        }

        public override  async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimeStamps();
            return await base.SaveChangesAsync(cancellationToken);
        }


        public override int SaveChanges()
        {
            AddTimeStamps();
            return  base.SaveChanges();
        }

        private void AddTimeStamps()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                var now = DateTime.Now;

                if(entry.State == EntityState.Added)
                    ((BaseEntity)entry.Entity).CreatedAt = now;
                
                    ((BaseEntity)entry.Entity).UpdatedAt = now;

            }
        
                }
    }
}
