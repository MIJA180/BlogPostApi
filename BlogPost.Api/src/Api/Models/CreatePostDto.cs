using System.ComponentModel.DataAnnotations;

namespace BlogPost.Api.src.Api.Models;

public record CreatePostDto(
    [Required][StringLength(50)] string Title,
    [Required][StringLength(100)] string Content,
    [Required][StringLength(30)] string Author,
    bool IsPublished
);