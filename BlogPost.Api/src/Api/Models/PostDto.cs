namespace BlogPost.Api.src.Api.Models;

public record PostDto(
    int Id,
    string Title,
    string Content,
    string Author,
    DateTime CreatedDate,
    DateTime? UpdatedDate,
    DateTime? PublishedDate,
    bool IsPublished
);