using Microsoft.EntityFrameworkCore;

namespace BlogPost.Api.src.Infrastructure.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogPostContext>();
        dbContext.Database.Migrate();
    }
}
