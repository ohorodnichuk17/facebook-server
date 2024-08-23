using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories;

public class Repository<T> : IRepository<T>
    where T : class
{
    private readonly FacebookDbContext _context;
    internal DbSet<T> dbSet;

    public Repository(FacebookDbContext context)
    {
        _context = context;
        this.dbSet = _context.Set<T>();
    }
    
    public async Task<ErrorOr<IEnumerable<T>>> GetAllAsync()
    {
        try
        {
            var entities = await dbSet.ToListAsync();
            return entities;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<T>> GetByIdAsync(Guid id)
    {
        try
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
            {
                return Error.Failure($"{typeof(T).Name} not found");
            }
            return entity;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<Guid>> CreateAsync(T entity)
    {
        try
        {
            dbSet.Add(entity);
            await _context.SaveChangesAsync();
            var entityId = (Guid)typeof(T).GetProperty("Id")!.GetValue(entity)!;
            return entityId;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> DeleteAsync(Guid id)
    {
        try
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
            {
                return Error.Failure($"{typeof(T).Name} not found");
            }
            dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<Unit>> SaveAsync(T entity)
    {
        try
        {
            dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
