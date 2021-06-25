using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class UserDto : UserBase
    {
        
        [Required]
        [StringLength(10, MinimumLength = 4)]
        public string Password { get; set; }

    }
}