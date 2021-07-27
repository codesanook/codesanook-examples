using Codesanook.Examples.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Codesanook.Examples.DotNet.Orm.EF6Examples.EntityConfigurations
{
    public class PostEntityConfiguration : EntityTypeConfiguration<Post>
    {
        public PostEntityConfiguration()
        {
            ToTable($"{nameof(Post).ToLower()}s");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Title).HasColumnName("title");
            Property(x => x.Details).HasColumnName("details");
            Property(x => x.BlogId).HasColumnName("blog_id");

            HasRequired(p => p.Blog).WithMany().HasForeignKey(x => x.BlogId);
            HasMany(x => x.Comments).WithRequired(x => x.Post).HasForeignKey(x => x.PostId);
        }
    }
}
