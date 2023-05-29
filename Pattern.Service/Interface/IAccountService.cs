using Pattern.DTO;
using Pattern.Entity.DataModels;

namespace Pattern.Service.Interface
{
    public interface IAccountService : IBaseService<User>
    {
        Task<UserDTO?> LoginAsync(LoginDTO user);
    }
}
