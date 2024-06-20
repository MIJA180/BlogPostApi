# Use the official ASP.NET image as a base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["BlogPost.Api/BlogPost.Api.csproj", "BlogPost.Api/"]
RUN dotnet restore "BlogPost.Api/BlogPost.Api.csproj"

# Copy the rest of the application and build
COPY . .
WORKDIR "/src/BlogPost.Api"
RUN dotnet build "BlogPost.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "BlogPost.Api.csproj" -c Release -o /app/publish

# Copy the build result to the base image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogPost.Api.dll"]
