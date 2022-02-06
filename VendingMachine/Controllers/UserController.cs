using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.Repository.Interfaces;
using VendingMachine.Repository.Models.User;

namespace VendingMachine.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("user/{id}")]
        public async Task<ActionResult<UserResponse>> GetAUser(string id)
        {
            return await _userRepository.GetUser(id);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("user")]
        public async Task<ActionResult<List<UserResponse>>> GetUsers()
        {
            return await _userRepository.GetUsers();
        }

        [HttpPost]
        [Route("user/register")]
        public async Task<ActionResult<AuthenticationResult>> CreateUser([FromBody] CreateUserDto user)
        {
            return await _userRepository.CreateUserAsync(user);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("user")]
        public async Task<ActionResult<AuthenticationResult>> UpdateUser([FromBody] UpdateUserDto user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("user")]
        public async Task<AuthenticationResult> DeleteUser(string userId)
        {
            return await _userRepository.DeleteUser(userId);
        }

        [HttpPost]
        [Route("user/login")]
        public async Task<ActionResult<AuthenticationResult>> Login([FromBody] LoginUserDto user)
        {
            return await _userRepository.LoginAsync(user);
        }


        [HttpPost]
        [Route("user/refresh")]
        public async Task<ActionResult<AuthenticationResult>> Refresh([FromBody] RefreshUserDto user)
        {
            return await _userRepository.RefreshTokenAsync(user);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("user/logout")]
        public async Task<ActionResult> Logout([FromBody] LogoutUserDto user)
        {
            await _userRepository.LogoutAsync(user);
            return Unauthorized();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("user/logout/all")]
        public async Task<ActionResult> LogoutAll([FromBody] LogoutAllUserDto user)
        {
            await _userRepository.LogoutAll(user);
            return Ok("Your active sessions have been logout");
        }
    }
}
