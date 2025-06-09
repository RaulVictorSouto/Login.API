using System.ComponentModel.DataAnnotations;

namespace Login.API.Request
{
    public class LoginRequestDto
    {
        [Required]
        public string Identifier { get; init; }

        [Required]
        public string Password { get; init; }
    }
}
