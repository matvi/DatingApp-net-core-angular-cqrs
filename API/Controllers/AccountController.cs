using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Commands;
using API.Data;
using Common.Dtos;
using Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _datacontext;
        private readonly ITokenService _tokenService;
        private readonly IMediator _mediator;

        public AccountController(
            DataContext datacontext,
            ITokenService tokenService,
            IMediator mediator)
        {
            _datacontext = datacontext;
            _tokenService = tokenService;
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserDto userDto)
        {
            var registerUserCommand = new RegisterUserCommand
            {
                UserDto = userDto
            };

            var result = await _mediator.Send(registerUserCommand);

            if (result == null)
            {
                return BadRequest("User already exits");
            }
            
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
    }
}