using Codesanook.Examples.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Codesanook.Examples.DotNetCore.Orm.EF6Examples.EntityConfigurations
{
    public class BlogEntityConfiguration : EntityTypeConfiguration<Blog>
    {
        public BlogEntityConfiguration()
        {
            ToTable($"{nameof(Blog).ToLower()}s");
            HasKey(b => b.Id);
            Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(b => b.Name).HasColumnName("name");
            Property(b => b.Description).HasColumnName("description");

            HasMany(b => b.Posts).WithRequired(p => p.Blog).HasForeignKey(x => x.BlogId);
        }
    }
}
