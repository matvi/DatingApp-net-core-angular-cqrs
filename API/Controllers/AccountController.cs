using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using Common.Dtos;
using Common.Entity;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _datacontext;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext datacontext, ITokenService tokenService)
        {
            _datacontext = datacontext;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserDto userDto)
        {
            userDto.UserName.ToLower();

            if(await UserExists(userDto.UserName))
            {
                return BadRequest("User already exits");
            }
            
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = userDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password)),
                PasswordSalt = hmac.Key
            };
            await _datacontext.Users.AddAsync(user);
            await _datacontext.SaveChangesAsync();

            return Created("urlToAPI", userDto);

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLogged>> Login (UserDto loginUser)
        {
            
            loginUser.UserName = loginUser.UserName.ToLower();
            var loggedUser = new UserLogged
            {
                UserName = loginUser.UserName
            };

            var user = await _datacontext.Users.FirstOrDefaultAsync(u => u.UserName.Equals(loginUser.UserName));

            if (user is null){
                return BadRequest("User not exitts");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginUser.Password));
            if (computedHash.Length != user.PasswordHash.Length)
            {
                return Unauthorized("Bad password");
            }

            for (int i = 0; i< computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) 
                {
                    return Unauthorized("Bad password");
                }
            }

            var token = _tokenService.CreateToken(user);
            loggedUser.Token = token;

            return loggedUser;
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _datacontext.Users.AnyAsync(u => u.UserName == userName);
        }

    }
}