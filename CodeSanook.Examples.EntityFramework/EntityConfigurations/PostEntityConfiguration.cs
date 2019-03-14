using CodeSanook.Examples.EntityFramework.Models;
using System.Data.Entity.ModelConfiguration;

namespace CodeSanook.Examples.EntityFramework.EntityConfigurations
{
    public class PostEntityConfiguration : EntityTypeConfiguration<Post>
    {
        public PostEntityConfiguration()
        {
            this.ToTable($"{nameof(Post)}s");
            this.HasKey<int>(x => x.Id);
            this.Property(p => p.Title);
            this.Property(p => p.Details);
            this.HasMany(p => p.Comments)
                .WithRequired(c=>c.Post);
        }
    }
}