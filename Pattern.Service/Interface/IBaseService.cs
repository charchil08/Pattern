using Pattern.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pattern.Service.Interface
{
    public interface IBaseService<T> where T : class
    {
        IQueryable<T> GetAllAsync();
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindByIdAsync(int id);
        Task AddAsync(T entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);

        //PageList<TDTO> GetList<TDTO>(FilterParams filterParams, Expression<Func<T, bool>>? filterExpression = null) where TDTO : class;
    }
}
