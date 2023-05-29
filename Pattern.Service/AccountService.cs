using AutoMapper;
using Pattern.DTO;
using Pattern.Entity.DataModels;
using Pattern.Repository.Interface;
using Pattern.Service.Interface;

namespace Pattern.Service
{
    public class AccountService : BaseService<User>, IAccountService
    {
        private readonly IAccountRepo _repo;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepo repo, IMapper mapper):base(repo) 
        {
            _repo = repo;
            _mapper = mapper;
        }


        public async Task<UserDTO?> LoginAsync(LoginDTO user)
        {
            var isUserExists = await _repo.FindByAsync(u => u.Email == user.Email); 
            if(isUserExists.Any())
            {
                var loggedinUser = await _repo.LoginAsync(user.Email, user.Password);
                if(loggedinUser != null)
                {
                    UserDTO userDTO = _mapper.Map<UserDTO>(loggedinUser);
                    return userDTO;
                }
            }
            return null;
        } 

    }
}
