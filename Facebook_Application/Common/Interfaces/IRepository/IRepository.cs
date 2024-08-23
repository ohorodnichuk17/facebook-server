using ErrorOr;
using MediatR;

namespace Facebook.Application.Common.Interfaces.IRepository;

public interface IRepository<T> where T : class
{
    Task<ErrorOr<IEnumerable<T>>> GetAllAsync();
    Task<ErrorOr<T>> GetByIdAsync(Guid id);
    Task<ErrorOr<Guid>> CreateAsync(T entity);
    Task<ErrorOr<bool>> DeleteAsync(Guid id);
    Task<ErrorOr<Unit>> SaveAsync(T entity);
}