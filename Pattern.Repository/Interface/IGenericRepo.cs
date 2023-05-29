using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pattern.Repository.Interface
{
    public interface IGenericRepo<T> where T : class
    {
        IQueryable<T> GetAllAsync();
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Delete(T entity);
        void Edit(T entity);
        Task SaveAsync();
        Task<T> FindByIdAsync(int id);
    }
}
