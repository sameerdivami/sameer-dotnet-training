using PolicyManagement.Entities;  
using System.Collections.Generic;
using System.Threading.Tasks;
using PolicyManagement.DTOs;
public interface IUserRepository
{
    Task<UserResponseDto> CreateUserAsync(UserCreateDto user);
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
    Task<User?> UpdateUserAsync(int id, User user);
    Task<bool> DeleteUserAsync(int id);
}