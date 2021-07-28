using System.ComponentModel.DataAnnotations;
using API.Dtos;

namespace Common.Dtos
{
    public class UserDto : UserBase
    {
        public string Password { get; set; }

    }
}