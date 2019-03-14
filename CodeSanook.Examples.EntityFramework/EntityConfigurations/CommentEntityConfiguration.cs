using CodeSanook.Examples.EntityFramework.Models;
using System.Data.Entity.ModelConfiguration;

namespace CodeSanook.Examples.EntityFramework.EntityConfigurations
{
    public class CommentEntityConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentEntityConfiguration()
        {
            this.ToTable($"{nameof(Comment)}s");
            this.HasKey<int>(x => x.Id);
            this.Property(p => p.Content);
            this.HasRequired(p => p.Post);
        }
    }
}