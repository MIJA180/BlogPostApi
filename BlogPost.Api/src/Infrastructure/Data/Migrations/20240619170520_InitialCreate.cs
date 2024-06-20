using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogPost.Api.src.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "Author", "Content", "CreatedDate", "IsPublished", "PublishedDate", "Title", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "John Doe", "This post covers the basics of C# programming language...", new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Introduction to C#", null },
                    { 2, "Jane Smith", "ASP.NET Core is a free and open-source web framework...", new DateTime(2023, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2023, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Understanding ASP.NET Core", new DateTime(2023, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Emily Johnson", "Entity Framework Core (EF Core) is a lightweight, extensible...", new DateTime(2023, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Getting Started with Entity Framework Core", null },
                    { 4, "Michael Brown", "This post explains how to deploy ASP.NET Core applications...", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2023, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deploying ASP.NET Core Applications", new DateTime(2023, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Jessica White", "In this post, we explore some advanced C# programming techniques...", new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Advanced C# Programming Techniques", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPosts");
        }
    }
}
