using BlogPost.Api.src.Core.Entities;
using BlogPost.Api.src.Core.Exceptions;
using BlogPost.Api.src.Core.Interfaces;
using BlogPost.Api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BlogPost.Api.src.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly BlogPostContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(BlogPostContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            Log.Debug($"Fetching all entities from the database.");
            return await _dbSet.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching all blog posts.");
            throw new RepositoryException("An error occurred while fetching all blog posts.", ex);
        }
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        try
        {
            Log.Debug($"Fetching entity with ID {id} from the database.");
            return await _dbSet.FindAsync(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error occurred while fetching blog post with ID {id}.");
            throw new RepositoryException($"An error occurred while fetching blog post with ID {id}.", ex);
        }
    }

    public async Task AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            Log.Debug("Added a new entity to the database.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while adding a new blog post.");
            throw new RepositoryException("An error occurred while adding a new blog post.", ex);
        }
    }

    public async Task UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            Log.Debug("Updated entity in the database.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error occurred while updating blog post.");
            throw new RepositoryException($"An error occurred while updating blog post.", ex);
        }
    }

    public async Task DeleteAsync(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            Log.Debug("Deleted entity from the database.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error occurred while deleting blog post.");
            throw new RepositoryException($"An error occurred while deleting blog post.", ex);
        }
    }
}