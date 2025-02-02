using System;                              // Importa funcionalidades básicas do .NET, como o tipo DateTime.
using System.ComponentModel.DataAnnotations; // Importa atributos de validação para os campos do DTO.

namespace UsuariosApi.Data.Dtos
{
    // DTO (Data Transfer Object) usado para transferir dados durante o cadastro de um novo usuário.
    public class CreateUsuarioDto
    {
        // Define que este campo é obrigatório.
        [Required]
        public string Username { get; set; }

        // Define que este campo é obrigatório.
        public DateTime DataNascimento { get; set; }

        // Define que este campo é obrigatório e indica que ele representa uma senha.
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Define que este campo é obrigatório e deve ser igual ao campo "Password".
        [Required]
        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        public string RePassword { get; set; }
    }
}
