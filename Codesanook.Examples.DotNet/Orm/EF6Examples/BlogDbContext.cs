using Codesanook.Examples.Core.Models;
using Codesanook.Examples.DotNet.Orm.EF6Examples.EntityConfigurations;
using System.Data.Entity;

namespace Codesanook.Examples.DotNet.Orm.EF6Examples
{
    [DbConfigurationType(typeof(EF6CodeConfiguration))]
    public class BlogDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public BlogDbContext(string connectionString) : base(connectionString) // in memory connection string
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<BlogDbContext>());
            Configuration.LazyLoadingEnabled = false;// Disable lazy loading
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BlogEntityConfiguration());
            modelBuilder.Configurations.Add(new PostEntityConfiguration());
            modelBuilder.Configurations.Add(new CommentEntityConfiguration());
        }
    }
}
