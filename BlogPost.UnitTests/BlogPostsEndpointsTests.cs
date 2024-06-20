using BlogPost.Api.src.Api.Endpoints;
using BlogPost.Api.src.Api.Models;
using BlogPost.Api.src.Core.Entities;
using BlogPost.Api.src.Core.Exceptions;
using BlogPost.Api.src.Core.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace BlogPost.UnitTests;

public class BlogPostsEndpointsTests
{
    private readonly Mock<IBlogPostService> _mockBlogPostService;

    public BlogPostsEndpointsTests()
    {
        _mockBlogPostService = new Mock<IBlogPostService>();
    }

    [Fact]
    public async Task GetAllBlogPosts_ShouldReturnOkResultWithPosts()
    {
        // Arrange
        List<PostDto> fakePosts =
            [
                new PostDto(1, "Title1", "Content1", "Author1", DateTime.UtcNow, null, null, false),
                new PostDto(2, "Title2", "Content2", "Author2", DateTime.UtcNow, null, null, false)
            ];

        _mockBlogPostService.Setup(service => service.GetAllBlogPostsAsync())
            .ReturnsAsync(fakePosts);

        // Act
        var result = await BlogPostsEndpoints.GetAllBlogPosts(_mockBlogPostService.Object);

        // Assert
        var okResult = result as Ok<IEnumerable<PostDto>>;
        okResult.Should().NotBeNull();
        okResult?.Value.Should().BeEquivalentTo(fakePosts);
    }

    [Fact]
    public async Task GetBlogPostById_ShouldReturnOkResultWithPost()
    {
        // Arrange
        var fakePost = new PostDto(1, "Title1", "Content1", "Author1", DateTime.UtcNow, null, null, false);
        _mockBlogPostService.Setup(service => service.GetBlogPostByIdAsync(1))
            .ReturnsAsync(fakePost);

        // Act
        var result = await BlogPostsEndpoints.GetBlogPostById(1, _mockBlogPostService.Object);

        // Assert
        var okResult = result as Ok<PostDto>;
        okResult.Should().NotBeNull();

        okResult?.Value.Should().BeEquivalentTo(fakePost);
    }

    [Fact]
    public async Task GetBlogPostById_ShouldReturnNotFound_WhenPostDoesNotExist()
    {
        // Arrange
        _mockBlogPostService.Setup(service => service.GetBlogPostByIdAsync(1))
            .ReturnsAsync((PostDto?)null);

        // Act
        var result = await BlogPostsEndpoints.GetBlogPostById(1, _mockBlogPostService.Object);

        // Assert
        var notFoundResult = result as NotFound<string>;
        notFoundResult.Should().NotBeNull();

        notFoundResult?.Value.Should().Be($"Blog Post with ID 1 not found.");
    }

    [Fact]
    public async Task CreateBlogPost_ShouldReturnCreatedResult()
    {
        // Arrange
        var createPostDto = new CreatePostDto("New Title", "New Content", "New Author", true);
        var createdPost = new Post
        {
            Id = 1,
            Title = createPostDto.Title,
            Content = createPostDto.Content,
            Author = createPostDto.Author,
            CreatedDate = DateTime.UtcNow,
            IsPublished = createPostDto.IsPublished
        };

        _mockBlogPostService.Setup(service => service.AddBlogPostAsync(createPostDto))
            .ReturnsAsync(createdPost);

        // Act
        var result = await BlogPostsEndpoints.CreateBlogPost(createPostDto, _mockBlogPostService.Object);

        // Assert
        var createdAtRouteResult = result as CreatedAtRoute<Post>;
        createdAtRouteResult.Should().NotBeNull();

        if (createdAtRouteResult != null)
        {
            createdAtRouteResult.Value.Should().BeEquivalentTo(createdPost);
            createdAtRouteResult.RouteName.Should().Be("GetBlogPostById");
            createdAtRouteResult.RouteValues["id"].Should().Be(createdPost.Id);
        }
    }

    [Fact]
    public async Task CreateBlogPost_ShouldReturnBadRequest_WhenDataIsInvalid()
    {
        // Arrange
        var invalidCreatePostDto = new CreatePostDto("", "", "", false);

        _mockBlogPostService.Setup(service => service.AddBlogPostAsync(It.IsAny<CreatePostDto>()))
                            .ReturnsAsync((CreatePostDto dto) =>
                            {
                                return new Post
                                {
                                    Id = 1,
                                    Title = dto.Title,
                                    Content = dto.Content,
                                    Author = dto.Author,
                                    IsPublished = dto.IsPublished
                                };
                            });

        // Act
        var result = await BlogPostsEndpoints.CreateBlogPost(invalidCreatePostDto, _mockBlogPostService.Object);

        // Assert
        var badRequestResult = result as BadRequest<string>;

        // Verify specific validation error message based on your validation attributes
        badRequestResult?.Value.Should().Contain("The Title field is required.")
                                     .And.Contain("The Content field is required.")
                                     .And.Contain("The Author field is required.");
    }

    [Fact]
    public async Task UpdateBlogPost_ShouldReturnNotFound_WhenPostDoesNotExist()
    {
        // Arrange
        int postId = 1;

        var updatePostDto = new UpdatePostDto(
            "Dummy Title",
            "Dummy Content",
            "Dummy Author",
            null,
            false
        );


        _mockBlogPostService.Setup(service => service.GetBlogPostByIdAsync(postId))
            .ReturnsAsync((PostDto?)null);

        // Act
        var result = await BlogPostsEndpoints.UpdateBlogPost(postId, updatePostDto, _mockBlogPostService.Object);

        // Assert
        var notFoundResult = result as NotFound<string>;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Be($"Blog Post with ID {postId} not found.");
    }


    [Fact]
    public async Task UpdateBlogPost_ShouldReturnNoContent_WhenPostExists()
    {
        // Arrange
        int postId = 1;
        var updatePostDto = new UpdatePostDto("Updated Title", "Updated Content", "Updated Author", null, true);
        var existingPost = new PostDto(
            Id: postId,
            Title: "Sample Title",
            Content: "Sample Content",
            Author: "Sample Author",
            CreatedDate: DateTime.Now,
            UpdatedDate: null,
            PublishedDate: null,
            IsPublished: false
        );


        _mockBlogPostService.Setup(service => service.GetBlogPostByIdAsync(postId))
            .ReturnsAsync(existingPost);

        // Act
        var result = await BlogPostsEndpoints.UpdateBlogPost(postId, updatePostDto, _mockBlogPostService.Object);

        // Assert
        result.Should().BeOfType<NoContent>();

        // Verify that UpdateBlogPostAsync was called with the correct parameters
        _mockBlogPostService.Verify(service => service.UpdateBlogPostAsync(postId, updatePostDto), Times.Once);
    }

    [Fact]
    public async Task DeleteBlogPost_ShouldReturnNoContent_WhenPostExists()
    {
        // Arrange
        int postId = 1;
        var existingPost = new PostDto(
                Id: postId,
                Title: "Sample Title",
                Content: "Sample Content",
                Author: "Sample Author",
                CreatedDate: DateTime.Now,
                UpdatedDate: null,
                PublishedDate: null,
                IsPublished: false
            );

        _mockBlogPostService.Setup(service => service.GetBlogPostByIdAsync(postId))
            .ReturnsAsync(existingPost);

        // Act
        var result = await BlogPostsEndpoints.DeleteBlogPost(postId, _mockBlogPostService.Object);

        // Assert
        result.Should().BeOfType<NoContent>();

        // Verify that DeleteBlogPostAsync was called with the correct postId
        _mockBlogPostService.Verify(service => service.DeleteBlogPostAsync(postId), Times.Once);
    }

    [Fact]
    public async Task DeleteBlogPost_ShouldReturnNotFound_WhenPostDoesNotExist()
    {
        // Arrange
        int postId = 1;
        _mockBlogPostService.Setup(service => service.GetBlogPostByIdAsync(postId))
            .ReturnsAsync((PostDto?)null);

        // Act
        var result = await BlogPostsEndpoints.DeleteBlogPost(postId, _mockBlogPostService.Object);

        // Assert
        var notFoundResult = result as NotFound<string>;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Be($"Blog Post with ID {postId} not found.");

        // Verify that DeleteBlogPostAsync was not called
        _mockBlogPostService.Verify(service => service.DeleteBlogPostAsync(It.IsAny<int>()), Times.Never);
    }

}

