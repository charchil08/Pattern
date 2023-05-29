using Pattern.DTO;
using Pattern.Repository.Interface;
using Pattern.Service.Interface;
using System.Linq.Expressions;

namespace Pattern.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {

        private readonly IGenericRepo<T> _repo;

        public BaseService(IGenericRepo<T> repo)
        {
            _repo = repo;
        }

        public async Task AddAsync(T entity)
        {
            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await _repo.FindByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            T entity = await _repo.FindByIdAsync(id);
             _repo.Delete(entity); 
            await _repo.SaveAsync();
        }

        public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repo.FindByAsync(predicate);
        }

        public IQueryable<T> GetAllAsync()
        {
            return _repo.GetAllAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _repo.Edit(entity);
            await _repo.SaveAsync();
        }


    }
}
