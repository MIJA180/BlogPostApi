namespace BlogPost.Api.src.Core.Entities;

public class Post
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Author { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? PublishedDate { get; set; }
    public bool IsPublished { get; set; }
}
