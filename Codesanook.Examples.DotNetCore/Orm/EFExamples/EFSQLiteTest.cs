using Codesanook.Examples.Core.Models;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.Data.Entity;

namespace Codesanook.Examples.DotNetCore.Orm.EFExamples
{
    public class EFSQLiteTest : IAsyncLifetime
    {
        private BlogDbContext db;

        public async Task InitializeAsync()
        {
            // In memory SQLite which get deleted after close a connection
            db = new BlogDbContext("Data Source=:memory:;Version=3;New=True;");

            // SQLite EF not create a table for you 
            // https://stackoverflow.com/a/23128288/1872200
            await CreateTables(db);
        }

        public Task DisposeAsync()
        {
            db.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task EagerLoadWithInclude_ValidQuery_ReturnIncludedEntities()
        {
            var blog = new Blog() { Name = "EF", Description = "Blog about EF" };
            var post = new Post() { Title = "EF Mapping", Details = "Learn EF Mapping" };
            blog.Posts.Add(post);

            var comment = new Comment() { Content = "Good post" };
            post.Comments.Add(comment);

            db.Blogs.Add(blog);
            await db.SaveChangesAsync();

            // https://docs.microsoft.com/en-us/ef/ef6/querying/related-data#eagerly-loading-multiple-levels
            // Eagerly loading multiple levels
            var blogsWithPostsAndComments =
                db.Blogs.Where(b => b.Name == "EF")
                .Include(b => b.Posts.Select(p => p.Comments))
                .ToList();

            Assert.Single(blogsWithPostsAndComments);
            Assert.Equal(1, blogsWithPostsAndComments.Single().Id);

            var allPosts = blogsWithPostsAndComments.SelectMany(b => b.Posts);
            Assert.Single(allPosts);

            var allComments = blogsWithPostsAndComments.SelectMany(b => b.Posts.SelectMany(p => p.Comments));
            Assert.Single(allComments);
        }

        [Fact]
        public async Task LeftJoinQuery_ValidQuery_ReturnBlogPostWithEmptyComment()
        {
            var blog = new Blog() { Name = "EF", Description = "Blog about EF" };
            var post = new Post() { Title = "EF Mapping", Details = "Learn EF Mapping" };
            blog.Posts.Add(post);

            db.Blogs.Add(blog);
            await db.SaveChangesAsync();

            // left join query
            var postWithLeftJoinComment =
               from b in db.Blogs
               join p in db.Posts
               on b.Id equals p.BlogId

               join c in db.Comments
               on p.Id equals c.PostId into comments

               from cc in comments.DefaultIfEmpty()
               select new { postTitle = p.Title, blogName = b.Name, comment = cc.Content };

            Assert.Single(postWithLeftJoinComment);
        }

        private static async Task CreateTables(BlogDbContext db)
        {
            var sqlCommand = await File.ReadAllTextAsync("Orm/create-blog-tables.sql");
            await db.Database.ExecuteSqlCommandAsync(sqlCommand);
            var userTableNames = await db.Database.SqlQuery<string>(
                @"SELECT name FROM sqlite_master WHERE type ='table' AND name NOT LIKE 'sqlite_%';"
            ).ToListAsync();

            Assert.Equal(3, userTableNames.Count);
        }
    }
}
