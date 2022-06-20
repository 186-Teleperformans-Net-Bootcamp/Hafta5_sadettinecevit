using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Common;
using SocialNetwork.Persistence.Context;

namespace SocialNetwork.Persistence.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IBaseEntity, new()
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        private DbSet<T> Table { get => _context.Set<T>(); }

        public virtual async Task<EntityEntry<T>> Add(T entity)
        {
            EntityEntry<T> result = await Table.AddAsync(entity);
            var retVal = await _context.SaveChangesAsync();
            return result;
        }

        public virtual async Task<EntityEntry<T>> Update(T entity)
        {
            EntityEntry<T> result = Table.Update(entity);
            await _context.SaveChangesAsync();
            return result;
        }

        public virtual async Task<EntityEntry<T>> Delete(T entity)
        {
            EntityEntry<T> result = Table.Remove(entity);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<List<T>> GetAsync() => await Table.ToListAsync();

        public async Task<T> GetByIdAsync(string id) => await Table.FindAsync(id);
    }
}
