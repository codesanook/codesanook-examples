using Codesanook.Examples.EntityFramework.Models;
using System.Data.Entity.ModelConfiguration;

namespace Codesanook.Examples.EntityFramework.EntityConfigurations
{
    public class CommentEntityConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentEntityConfiguration()
        {
            this.ToTable($"{nameof(Comment)}s");
            this.HasKey<int>(x => x.Id);
            this.Property(c => c.Content);
            this.HasRequired(c => c.Post).WithMany().HasForeignKey(c => c.PostId); ;
        }
    }
}