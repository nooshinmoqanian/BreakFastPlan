using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositories<Users> _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
      
        public UserService(IRepositories<Users> repositories, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = repositories;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<bool> IsUserAuthenticatedAsync(LoginDto loginDto)
        {
            if (!string.IsNullOrEmpty(loginDto.username))
            {
                var findUser = await _userRepository.GetByNameAsync(loginDto.username);

                if (findUser != null && !string.IsNullOrEmpty(loginDto.password))
                {
                    var verifyPass = BCrypt.Net.BCrypt.Verify(loginDto.password, findUser.Password);

                    return true;
                }
            }

            return false;
        }

        public Task<Users> GetUserById(int id)
        {
            return _userRepository.GetByIdAsync(id);
        }

        public async Task<Result> LoginUser(LoginDto loginDto)
        {
            if (loginDto != null)
            {
                var userMap = _mapper.Map<Users>(loginDto);

                var AccessToken = _tokenService.GenerateAccessToken(loginDto.username, loginDto.Role);

                var RefreshToken = _tokenService.GenerateRefreshToken(loginDto.username, loginDto.Role);

                var chekeVerify = await IsUserAuthenticatedAsync(loginDto);

                if(chekeVerify)
                    return new Result { Success = true, Message = "you are login", AccessToken = AccessToken, RefreshToken = RefreshToken };
            }

            return new Result { Success = false, Message = "The password is incorrect" };
        }

        public async Task<Result> RegisterUser(RegisterDto registerDto)
        {
            var userMap = _mapper.Map<Users>(registerDto);

            var AccessToken = _tokenService.GenerateAccessToken(registerDto.Username, registerDto.Role);

            var RefreshToken = _tokenService.GenerateRefreshToken(registerDto.Username, registerDto.Role);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            userMap.Password = hashedPassword;

            userMap.Token = RefreshToken;
            
            var resultRegister = await _userRepository.CreateAsync(userMap);

            if (resultRegister.Success == false)
                return new Result { Success = resultRegister.Success, Message = "User Registration Not Successful." };

            return new Result { Success = true, Message = "User Registration Successful", AccessToken = AccessToken, RefreshToken = RefreshToken };
        }
    }
}
