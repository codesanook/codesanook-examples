using Codesanook.Examples.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Codesanook.Examples.DotNetCore.Orm.EF6Examples.EntityConfigurations
{
    public class CommentEntityConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentEntityConfiguration()
        {
            ToTable($"{nameof(Comment).ToLower()}s");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Content).HasColumnName("content");
            Property(x => x.PostId).HasColumnName("post_id");

            HasRequired(x => x.Post).WithMany().HasForeignKey(x => x.PostId);
        }
    }
}
