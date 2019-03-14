using System.Collections.Generic;

namespace CodeSanook.Examples.EntityFramework.Models
{
    public class Post
    {
        public Post()
        {
            Comments = new List<Comment>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
