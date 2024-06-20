using BlogPost.Api.src.Api.Models;
using BlogPost.Api.src.Core.Entities;

namespace BlogPost.Api.src.Core.Interfaces;

public interface IBlogPostService
{
    Task<IEnumerable<PostDto>> GetAllBlogPostsAsync();
    Task<PostDto?> GetBlogPostByIdAsync(int id);
    Task<Post> AddBlogPostAsync(CreatePostDto createPostDto);
    Task UpdateBlogPostAsync(int id, UpdatePostDto updatePostDto);
    Task<bool> DeleteBlogPostAsync(int id);
}
