using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Repository.Models.User;

namespace VendingMachine.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<AuthenticationResult> CreateUserAsync(CreateUserDto user);
        Task<AuthenticationResult> UpdateUserAsync(UpdateUserDto user);
        Task<UserResponse> GetUser(string userId);
        Task<List<UserResponse>> GetUsers();
        Task<AuthenticationResult> DeleteUser(string userId);
        Task<AuthenticationResult> LoginAsync(LoginUserDto user);
        Task<AuthenticationResult> RefreshTokenAsync(RefreshUserDto user);
        Task LogoutAsync(LogoutUserDto user);
        Task<decimal> UpdateDeposit(string email, decimal deposit);
        Task LogoutAll(LogoutAllUserDto user);
    }
}
