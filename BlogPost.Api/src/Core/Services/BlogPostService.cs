using BlogPost.Api.src.Api.Models;
using BlogPost.Api.src.Core.Entities;
using BlogPost.Api.src.Core.Exceptions;
using BlogPost.Api.src.Core.Interfaces;
using BlogPost.Api.src.Core.Mappers;
using Serilog;

namespace BlogPost.Api.src.Core.Services;

public class BlogPostService(IRepository<Post> blogPostRepository) : IBlogPostService
{
    private readonly IRepository<Post> _blogPostRepository = blogPostRepository;

    public async Task<IEnumerable<PostDto>> GetAllBlogPostsAsync()
    {
        try
        {
            Log.Debug("Fetching all blog posts from the repository.");
            IEnumerable<Post> blogPosts = await _blogPostRepository.GetAllAsync();
            Log.Debug($"Retrieved {blogPosts.Count()} blog posts.");
            return blogPosts.Select(PostMapper.ToDto);
        }
        catch (RepositoryException ex)
        {
            Log.Error(ex, "RepositoryException occurred in GetAllBlogPostsAsync.");
            throw new BlogPostServiceException("An error occurred while retrieving all blog posts.", ex);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected exception occurred in GetAllBlogPostsAsync.");
            throw new BlogPostServiceException("An unexpected error occurred while retrieving all blog posts.", ex);
        }
    }

    public async Task<PostDto?> GetBlogPostByIdAsync(int id)
    {
        try
        {
            Log.Debug($"Fetching blog post with ID {id} from the repository.");
            Post? blogPost = await _blogPostRepository.GetByIdAsync(id);
            if (blogPost != null)
            {
                Log.Debug($"Retrieved blog post: {blogPost}");
                return PostMapper.ToDto(blogPost);
            }
            else
            {
                Log.Debug($"Blog post with ID {id} not found.");
                return null;
            }
        }
        catch (RepositoryException ex)
        {
            Log.Error(ex, $"RepositoryException occurred in GetBlogPostByIdAsync for ID {id}.");
            throw new BlogPostServiceException($"An error occurred while retrieving blog post with ID {id}.", ex);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Unexpected exception occurred in GetBlogPostByIdAsync for ID {id}.");
            throw new BlogPostServiceException($"An unexpected error occurred while retrieving blog post with ID {id}.", ex);
        }
    }

    public async Task<Post> AddBlogPostAsync(CreatePostDto createPostDto)
    {
        try
        {
            Log.Information($"Adding a new blog post: {createPostDto}.");
            Post blogPost = PostMapper.ToEntity(createPostDto);
            await _blogPostRepository.AddAsync(blogPost);
            Log.Debug($"Added a new blog post with ID {blogPost.Id}.");
            return blogPost;
        }
        catch (RepositoryException ex)
        {
            Log.Error(ex, "RepositoryException occurred in AddBlogPostAsync.");
            throw new BlogPostServiceException("An error occurred while adding a new blog post.", ex);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected exception occurred in AddBlogPostAsync.");
            throw new BlogPostServiceException("An unexpected error occurred while adding a new blog post.", ex);
        }
    }

    public async Task UpdateBlogPostAsync(int id, UpdatePostDto updatePostDto)
    {
        try
        {
            Log.Debug($"Updating blog post with ID {id}. New data: {updatePostDto}.");
            Post existingPost = await _blogPostRepository.GetByIdAsync(id) ??
                throw new NotFoundException($"Blog Post with ID {id} not found.");

            Log.Debug($"Current state of blog post: {existingPost}");

            PostMapper.UpdateEntity(existingPost, updatePostDto);
            await _blogPostRepository.UpdateAsync(existingPost);

            Log.Debug($"Updated blog post with ID {id}.");
        }
        catch (RepositoryException ex)
        {
            Log.Error(ex, $"RepositoryException occurred in UpdateBlogPostAsync for ID {id}.");
            throw new BlogPostServiceException($"An error occurred while updating blog post with ID {id}.", ex);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Unexpected exception occurred in UpdateBlogPostAsync for ID {id}.");
            throw new BlogPostServiceException($"An unexpected error occurred while updating blog post with ID {id}.", ex);
        }
    }

    public async Task<bool> DeleteBlogPostAsync(int id)
    {
        try
        {
            Log.Debug($"Deleting blog post with ID {id}.");

            Post? existingPost = await _blogPostRepository.GetByIdAsync(id);

            if (existingPost == null)
            {
                Log.Debug($"Blog post with ID {id} not found.");
                return false;
            }

            await _blogPostRepository.DeleteAsync(existingPost);
            Log.Debug($"Deleted blog post with ID {id}.");
            return true;
        }
        catch (RepositoryException ex)
        {
            Log.Error(ex, $"RepositoryException occurred in DeleteBlogPostAsync for ID {id}.");
            throw new BlogPostServiceException($"An error occurred while deleting blog post with ID {id}.", ex);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Unexpected exception occurred in DeleteBlogPostAsync for ID {id}.");
            throw new BlogPostServiceException($"An unexpected error occurred while deleting blog post with ID {id}.", ex);
        }
    }
}