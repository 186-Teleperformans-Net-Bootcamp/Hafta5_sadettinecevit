using SocialNetwork.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : IBaseEntity, new()
    {
        Task<List<T>> GetAsync();
        Task<T> GetByIdAsync(string id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
    }
}
