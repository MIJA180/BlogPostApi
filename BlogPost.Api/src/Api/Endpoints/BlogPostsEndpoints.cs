using BlogPost.Api.src.Api.Models;
using BlogPost.Api.src.Core.Entities;
using BlogPost.Api.src.Core.Exceptions;
using BlogPost.Api.src.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BlogPost.Api.src.Api.Endpoints;

public static class BlogPostsEndpoints
{
    public static RouteGroupBuilder MapBlogPostsEndpoints(this WebApplication webApplication)
    {
        RouteGroupBuilder group = webApplication.MapGroup("blogPosts");

        group.MapGet("/", GetAllBlogPosts)
            .WithName("GetAllBlogPosts")
            .WithSummary("Gets a list of all blog posts.")
            .WithDescription("Returns a list of blog posts.");

        group.MapGet("/{id}", GetBlogPostById)
            .WithName("GetBlogPostById")
            .WithSummary("Gets a blog post by its ID.")
            .WithDescription("Returns the requested blog post.")
            .Produces<PostDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateBlogPost)
            .WithName("CreateBlogPost")
            .WithSummary("Creates a new blog post.")
            .WithDescription("Creates a new blog post and returns the created post.")
            .Produces<PostDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithParameterValidation();

        group.MapPut("/{id}", UpdateBlogPost)
            .WithName("UpdateBlogPost")
            .WithSummary("Updates an existing blog post.")
            .WithDescription("Updates the specified blog post and returns no content.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithParameterValidation();

        group.MapDelete("/{id}", DeleteBlogPost)
            .WithName("DeleteBlogPost")
            .WithSummary("Deletes a blog post.")
            .WithDescription("Deletes the specified blog post and returns no content.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }

    // Endpoint implementations
    // GET /blogPosts
    public static async Task<IResult> GetAllBlogPosts(IBlogPostService blogPostService)
    {
        try
        {
            Log.Information("Fetching all blog posts.");
            IEnumerable<PostDto> posts = await blogPostService.GetAllBlogPostsAsync();
            Log.Information($"Fetched {posts.Count()} blog posts.");
            return Results.Ok(posts);
        }
        catch (BlogPostServiceException ex)
        {
            Log.Error(ex, "BlogPostServiceException occurred while fetching all blog posts.");
            return Results.BadRequest("Failed to fetch blog posts.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while fetching all blog posts.");
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }

    }

    // GET /blogPosts/1
    public static async Task<IResult> GetBlogPostById(int id, IBlogPostService blogPostService)
    {
        try
        {
            Log.Information($"Fetching blog post with ID {id}.");
            PostDto? post = await blogPostService.GetBlogPostByIdAsync(id);

            if (post is not null)
            {
                Log.Information($"Blog post with ID {id} found.");
                return Results.Ok(post);
            }
            else
            {
                Log.Warning($"Blog post with ID {id} not found.");
                return Results.NotFound($"Blog Post with ID {id} not found.");
            }
        }
        catch (BlogPostServiceException ex)
        {
            Log.Error(ex, $"BlogPostServiceException occurred while fetching blog post with ID {id}.");
            return Results.BadRequest($"Failed to fetch blog post with ID {id}.");
        }
        catch (NotFoundException ex)
        {
            Log.Warning(ex, $"Blog post with ID {id} not found.");
            return Results.NotFound($"Blog Post with ID {id} not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An unexpected error occurred while fetching blog post with ID {id}.");
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    // POST /blogPosts
    public static async Task<IResult> CreateBlogPost(CreatePostDto createPostDto, IBlogPostService blogPostService)
    {
        try
        {
            Log.Information("Creating a new blog post.");

            if (createPostDto is null)
            {
                Log.Warning("Invalid post data.");
                return Results.BadRequest("Invalid post data.");
            }

            Post createdPost = await blogPostService.AddBlogPostAsync(createPostDto);
            Log.Information($"Blog post created with ID {createdPost.Id}.");
            return Results.CreatedAtRoute("GetBlogPostById", new { id = createdPost.Id }, createdPost);
        }
        catch (BlogPostServiceException ex)
        {
            Log.Error(ex, "BlogPostServiceException occurred while creating a new blog post.");
            return Results.BadRequest("Failed to create a new blog post.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unexpected error occurred while creating a new blog post.");
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }

    }

    // PUT /blogPosts
    public static async Task<IResult> UpdateBlogPost(int id, UpdatePostDto updatePostDto, IBlogPostService blogPostService)
    {
        try
        {
            Log.Information($"Updating blog post with ID {id}.");

            PostDto? existingPost = await blogPostService.GetBlogPostByIdAsync(id);
            if (existingPost is null)
            {
                Log.Warning($"Blog post with ID {id} not found.");
                return Results.NotFound($"Blog Post with ID {id} not found.");
            }

            await blogPostService.UpdateBlogPostAsync(id, updatePostDto);
            Log.Information($"Blog post with ID {id} updated.");
            return Results.NoContent();
        }
        catch (BlogPostServiceException ex)
        {
            Log.Error(ex, $"BlogPostServiceException occurred while updating blog post with ID {id}.");
            return Results.BadRequest($"Failed to update blog post with ID {id}.");
        }
        catch (NotFoundException ex)
        {
            Log.Warning(ex, $"Blog post with ID {id} not found.");
            return Results.NotFound($"Blog Post with ID {id} not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An unexpected error occurred while updating blog post with ID {id}.");
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    // DELETE /blogPosts/1
    public static async Task<IResult> DeleteBlogPost(int id, IBlogPostService blogPostService)
    {
        try
        {
            Log.Information($"Deleting blog post with ID {id}.");

            PostDto? existingPost = await blogPostService.GetBlogPostByIdAsync(id);
            if (existingPost is null)
            {
                Log.Warning($"Blog post with ID {id} not found.");
                return Results.NotFound($"Blog Post with ID {id} not found.");
            }

            await blogPostService.DeleteBlogPostAsync(id);
            Log.Information($"Blog post with ID {id} deleted.");
            return Results.NoContent();
        }
        catch (BlogPostServiceException ex)
        {
            Log.Error(ex, $"BlogPostServiceException occurred while deleting blog post with ID {id}.");
            return Results.BadRequest($"Failed to delete blog post with ID {id}.");
        }
        catch (NotFoundException ex)
        {
            Log.Warning(ex, $"Blog post with ID {id} not found.");
            return Results.NotFound($"Blog Post with ID {id} not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An unexpected error occurred while deleting blog post with ID {id}.");
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
