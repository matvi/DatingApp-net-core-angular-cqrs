using System.ComponentModel.DataAnnotations;
using API.Dtos;

namespace Common.Dtos
{
    public class UserDto : UserBase
    {
        
        [Required]
        [StringLength(10, MinimumLength = 4)]
        public string Password { get; set; }

    }
}