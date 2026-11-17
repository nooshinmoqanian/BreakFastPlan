using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using BusinessLogic.Validators;
using DataAccess.Models;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositories<Users> _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public UserService(IRepositories<Users> repositories, IMapper mapper, ITokenService tokenService, IRoleService roleService, JwtSettings jwtSettings)
        {
            _userRepository = repositories;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleService = roleService;
            _jwtSettings = jwtSettings;
        }

        public async Task<bool> IsUserAuthenticatedAsync(LoginDto loginDto)
        {
            if (!string.IsNullOrEmpty(loginDto.username))
            {
                var findUser = await _userRepository.GetByNameAsync(loginDto.username);

                if (findUser.Id != 0 && !string.IsNullOrEmpty(loginDto.password))
                {
                    return BCrypt.Net.BCrypt.Verify(loginDto.password, findUser.Password);
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
                var chekeVerify = await IsUserAuthenticatedAsync(loginDto);

                if (chekeVerify)
                {
                    var findUser = await _userRepository.GetByNameAsync(loginDto.username);

                    var AccessToken = _tokenService.GenerateAccessToken(loginDto.username, findUser.Role);

                    var RefreshToken = _tokenService.GenerateRefreshToken(loginDto.username, findUser.Role);

                    return new Result { Success = true, Message = "you are login", AccessToken = AccessToken, RefreshToken = RefreshToken };
                }
            }

            return new Result { Success = false, Message = "نام کاربری یا رمز عبور اشتباه است" };
        }

        public async Task<Result> RegisterUser(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetByNameAsync(registerDto.Username);

            if (existingUser.Id != 0)
                return new Result { Success = false, Message = "این نام کاربری قبلاً ثبت شده است" };

            var userMap = _mapper.Map<Users>(registerDto);

            var AccessToken = _tokenService.GenerateAccessToken(registerDto.Username, "User");

            var RefreshToken = _tokenService.GenerateRefreshToken(registerDto.Username, "User");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            userMap.Password = hashedPassword;

            userMap.Token = RefreshToken;

            var resultRegister = await _userRepository.CreateAsync(userMap);

            if (resultRegister.Success == false)
                return new Result { Success = resultRegister.Success, Message = "ثبت‌نام انجام نشد" };

            return new Result { Success = true, Message = "User Registration Successful", AccessToken = AccessToken, RefreshToken = RefreshToken };
        }
    }
}
