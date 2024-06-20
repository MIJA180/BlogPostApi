using BlogPost.Api.src.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogPost.Api.src.Infrastructure.Data;

public class BlogPostContext(DbContextOptions<BlogPostContext> options) : DbContext(options)
{
    public DbSet<Post> BlogPosts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>().HasData(
            new Post
            {
                Id = 1,
                Title = "Introduction to C#",
                Content = "This post covers the basics of C# programming language...",
                Author = "John Doe",
                CreatedDate = new DateTime(2023, 1, 15),
                UpdatedDate = null,
                PublishedDate = new DateTime(2023, 1, 20),
                IsPublished = true
            },
            new Post
            {
                Id = 2,
                Title = "Understanding ASP.NET Core",
                Content = "ASP.NET Core is a free and open-source web framework...",
                Author = "Jane Smith",
                CreatedDate = new DateTime(2023, 2, 10),
                UpdatedDate = new DateTime(2023, 2, 12),
                PublishedDate = new DateTime(2023, 2, 15),
                IsPublished = true
            },
            new Post
            {
                Id = 3,
                Title = "Getting Started with Entity Framework Core",
                Content = "Entity Framework Core (EF Core) is a lightweight, extensible...",
                Author = "Emily Johnson",
                CreatedDate = new DateTime(2023, 3, 5),
                UpdatedDate = null,
                PublishedDate = null,
                IsPublished = false
            },
            new Post
            {
                Id = 4,
                Title = "Deploying ASP.NET Core Applications",
                Content = "This post explains how to deploy ASP.NET Core applications...",
                Author = "Michael Brown",
                CreatedDate = new DateTime(2023, 4, 1),
                UpdatedDate = new DateTime(2023, 4, 3),
                PublishedDate = new DateTime(2023, 4, 5),
                IsPublished = true
            },
            new Post
            {
                Id = 5,
                Title = "Advanced C# Programming Techniques",
                Content = "In this post, we explore some advanced C# programming techniques...",
                Author = "Jessica White",
                CreatedDate = new DateTime(2023, 5, 20),
                UpdatedDate = null,
                PublishedDate = new DateTime(2023, 5, 25),
                IsPublished = true
            }
        );
    }
}
