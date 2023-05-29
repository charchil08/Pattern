using Pattern.Entity.DataModels;

namespace Pattern.Repository.Interface
{
    public interface IAccountRepo : IGenericRepo<User>
    {

        Task<User?> LoginAsync(string email, string password);
    }
}
