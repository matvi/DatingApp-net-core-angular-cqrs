using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class UserBase
    {
        [Required]
        public string UserName { get; set; }

    }
}