using System.ComponentModel.DataAnnotations;

namespace Login.API.Request
{
    public record class UserRequest
    {
        [Required]
        public string Email { get; init; }

        [Required]
        public string Password { get; init; }
    }

}
