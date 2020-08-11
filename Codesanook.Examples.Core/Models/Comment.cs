namespace Codesanook.Examples.Core.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}
