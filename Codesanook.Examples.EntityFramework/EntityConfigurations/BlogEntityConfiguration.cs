using Codesanook.Examples.EntityFramework.Models;
using System.Data.Entity.ModelConfiguration;

namespace Codesanook.Ebamples.EntityFramework.EntityConfigurations
{
    public class BlogEntityConfiguration : EntityTypeConfiguration<Blog>
    {
        public BlogEntityConfiguration()
        {
            this.ToTable($"{nameof(Blog)}s");
            this.HasKey<int>(b => b.Id);
            this.Property(b => b.Name);
            this.Property(b => b.Description);
            this.HasMany(b => b.Posts)
                .WithRequired(p=>p.Blog);
        }
    }
}