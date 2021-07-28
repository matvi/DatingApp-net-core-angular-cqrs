using API.Dtos;

namespace Common.Dtos
{
    public class UserLogged : UserBase
    {
        public string Token { get; set; }
    }
}