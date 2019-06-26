using Codesanook.Ebamples.EntityFramework.EntityConfigurations;
using Codesanook.Examples.EntityFramework.EntityConfigurations;
using Codesanook.Examples.EntityFramework.Models;
using System.Data.Entity;

namespace Codesanook.Examples.EntityFramework
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public BlogDbContext() : base("BlogConnectionString")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<BlogDbContext>());
            //disable lazy loading
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BlogEntityConfiguration());
            modelBuilder.Configurations.Add(new PostEntityConfiguration());
            modelBuilder.Configurations.Add(new CommentEntityConfiguration());
        }
    }
}
