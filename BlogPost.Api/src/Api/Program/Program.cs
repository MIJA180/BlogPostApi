using BlogPost.Api.src.Api.Endpoints;
using BlogPost.Api.src.Core.Entities;
using BlogPost.Api.src.Core.Interfaces;
using BlogPost.Api.src.Core.Services;
using BlogPost.Api.src.Infrastructure.Data;
using BlogPost.Api.src.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
string? connectionString = builder.Configuration.GetConnectionString("BlogPost");
builder.Services.AddSqlite<BlogPostContext>(connectionString);

// Register services
builder.Services.AddScoped<IRepository<Post>, Repository<Post>>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BlogPost API",
        Version = "v1",
        Description = "A simple example ASP.NET Core Web API for managing blog posts",
        Contact = new OpenApiContact
        {
            Name = "Irfaan Auckburally",
            Email = "auckburallyirfaan@gmail.com"
        }
    });
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogPost API v1");
    c.RoutePrefix = string.Empty;
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Map endpoints
app.MapBlogPostsEndpoints();

// Migrate the database
app.MigrateDb();

app.Run();
