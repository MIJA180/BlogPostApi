using System.ComponentModel.DataAnnotations;

namespace BlogPost.Api.src.Api.Models;

public record UpdatePostDto(
    [Required][StringLength(50)] string Title,
    [Required][StringLength(100)] string Content,
    [Required][StringLength(30)] string Author,
    DateTime? PublishedDate,
    bool IsPublished
);