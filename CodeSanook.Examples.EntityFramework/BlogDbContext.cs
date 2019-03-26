using CodeSanook.Ebamples.EntityFramework.EntityConfigurations;
using CodeSanook.Examples.EntityFramework.EntityConfigurations;
using CodeSanook.Examples.EntityFramework.Models;
using System.Data.Entity;

namespace CodeSanook.Examples.EntityFramework
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
