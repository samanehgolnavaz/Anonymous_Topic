using Anonymous_Topics.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Anonymous_Topics.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TopicCategory> TopicCategories => base.Set<TopicCategory>();
        public DbSet<Topic> Topics => base.Set<Topic>();
        public DbSet<TopicComment> TopicComments => base.Set<TopicComment>();

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var projectAssembly = typeof(ApplicationDbContext).Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(projectAssembly);
        }
    }
}
