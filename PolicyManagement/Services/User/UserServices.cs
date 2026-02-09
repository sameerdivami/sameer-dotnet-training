using PolicyManagement.Entities;
using PolicyManagement.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using PolicyManagement.DTOs;
namespace PolicyManagement.Services
{
    public class UserServices : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<UserResponseDto> CreateUserAsync(UserCreateDto user)
        {
            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            return await _userRepository.UpdateUserAsync(id, user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }
    }
}