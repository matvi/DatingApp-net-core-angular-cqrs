using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class UserBase
    {
        [Required]
        public string UserName { get; set; }

    }
}