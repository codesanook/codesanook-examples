using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSanook.Examples.EntityFramework.Models
{
    public class Blog
    {
        public Blog()
        {
            Posts = new List<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
