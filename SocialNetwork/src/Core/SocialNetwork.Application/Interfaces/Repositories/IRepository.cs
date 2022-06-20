using Microsoft.EntityFrameworkCore.ChangeTracking;
using SocialNetwork.Domain.Common;

namespace SocialNetwork.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class, IBaseEntity, new()
    {
        Task<List<T>> GetAsync();
        Task<T> GetByIdAsync(string id);
        Task<EntityEntry<T>> Add(T entity);
        Task<EntityEntry<T>> Update(T entity);
        Task<EntityEntry<T>> Delete(T entity);
    }
}
