using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.user;


namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<Users> _userRepository;
        private readonly IMapper _mapper;

    public UserService(IRepository<Users> userRepository, IMapper mapper)
    {
            _userRepository = userRepository;
            _mapper = mapper;
    }

        public async Task<RegistrationResult> RegisterUser(RegisterDto registerDto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var userMap = _mapper.Map<Users>(registerDto);

            userMap.Password = hashedPassword;

            var resultRegister = await _userRepository.Add(userMap);
             
             if(resultRegister) 
               return new RegistrationResult { Success = true, Message = "User registration successful." };

             return new RegistrationResult { Success = true, Message = "User Not registered" };
        }
    }
}
