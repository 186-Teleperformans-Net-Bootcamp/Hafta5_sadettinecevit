using Microsoft.EntityFrameworkCore;
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

        public virtual async Task<T> Add(T entity)
        {
            await Table.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> Update(T entity)
        {
            Table.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> Delete(T entity)
        {
            Table.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> GetAsync() => await Table.ToListAsync();

        public async Task<T> GetByIdAsync(string id) => await Table.FindAsync(id);
    }
}
