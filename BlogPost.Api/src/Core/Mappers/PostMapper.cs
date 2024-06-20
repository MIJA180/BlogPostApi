using BlogPost.Api.src.Api.Models;
using BlogPost.Api.src.Core.Entities;

namespace BlogPost.Api.src.Core.Mappers;

public static class PostMapper
{
    public static PostDto ToDto(Post post) =>
        new(
            post.Id,
            post.Title,
            post.Content,
            post.Author,
            post.CreatedDate,
            post.UpdatedDate,
            post.PublishedDate,
            post.IsPublished
        );

    public static Post ToEntity(CreatePostDto createPostDto)
    {
        return new Post
        {
            Title = createPostDto.Title,
            Content = createPostDto.Content,
            Author = createPostDto.Author,
            IsPublished = createPostDto.IsPublished,
            CreatedDate = DateTime.Now
        };
    }

    public static void UpdateEntity(Post entity, UpdatePostDto updatePostDto)
    {
        entity.Title = updatePostDto.Title;
        entity.Content = updatePostDto.Content;
        entity.Author = updatePostDto.Author;
        entity.IsPublished = updatePostDto.IsPublished;
        entity.PublishedDate = updatePostDto.PublishedDate;
        entity.UpdatedDate = DateTime.Now;
    }
}
