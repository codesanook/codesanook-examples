using CodeSanook.Examples.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSanook.Examples.EntityFramework
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (var db = new BlogDbContext())
            {
                var blog = new Blog() { Name = "EF", Description = "Blog about EF" };
                var post = new Post() { Title = "EF Mapping", Details = "Learn EF Mapping" };
                blog.Posts.Add(post);

                var comment = new Comment() { Content = "Good post" };
                post.Comments.Add(comment);

                db.Blogs.Add(blog);

                db.SaveChanges();
                Console.WriteLine($"{db.Posts.Count()}");
            }

            IList<Blog> blogs;
            using (var db = new BlogDbContext())
            {
                blogs = db.Blogs.Where(b => b.Name == "EF")
                     .Include(b => b.Posts.Select(p => p.Comments))
                     .ToList();
            }

        }
    }
}
