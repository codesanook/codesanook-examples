using Codesanook.Examples.EntityFramework.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Codesanook.Examples.EntityFramework
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (var db = new BlogDbContext())
            {
                //var blog = new Blog() { Name = "EF", Description = "Blog about EF" };
                //var post = new Post() { Title = "EF Mapping", Details = "Learn EF Mapping" };
                //blog.Posts.Add(post);

                //var comment = new Comment() { Content = "Good post" };
                //post.Comments.Add(comment);

                //db.Blogs.Add(blog);
                //db.SaveChanges();

               // var result = from b in db.Blogs
               //              join p in db.Posts
               //              on b.Id equals p.BlogId
               //              join c in db.Comments
               //              on p.Id equals c.PostId into comments
               //              from cc in comments.DefaultIfEmpty()
               //              select new { BlogId = b.Id };

               //var list = result.ToList();
            }

            IList<Blog> blogs;
            using (var db = new BlogDbContext())
            {
                blogs = db.Blogs.Where(b => b.Name == "EF")
                     .Include(b => b.Posts.Select(p => p.Comments))
                     .ToList();
            }
            var post = blogs.First().Posts;
        }
    }
}
