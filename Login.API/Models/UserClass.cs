using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Login.API.Models
{
    public class UserClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O username é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O username deve ter entre 3 e 100 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9_\-.]*$", ErrorMessage = "Username só pode conter letras, números, pontos, hífens e underlines")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [StringLength(255, ErrorMessage = "O email não pode exceder 255 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
        [MaxLength(100, ErrorMessage = "A senha não pode exceder 100 caracteres")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "A senha deve conter pelo menos: 1 letra maiúscula, 1 minúscula, 1 número e 1 caractere especial")]
        [NotMapped] // Não será armazenado no banco - apenas para validação
        public string Password { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Adicionais para confirmação de email
        public bool EmailConfirmed { get; set; } = false;
        public string EmailConfirmationToken { get; set; }
        public DateTime? EmailConfirmationTokenExpiry { get; set; }

        // Adicionais para reset de senha
        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
    }
}
