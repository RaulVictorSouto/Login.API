using System.ComponentModel.DataAnnotations;

namespace Login.API.Request
{
    public record class RegisterRequest
    {
        [Required]
        public string Email { get; init; }
        [Required]
        public string Username { get; init; }

        [Required]
        public string Password { get; init; }
    }

}
