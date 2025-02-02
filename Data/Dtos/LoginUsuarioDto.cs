using System.ComponentModel.DataAnnotations; // Importa atributos de validação para os campos do DTO.

namespace UsuariosApi.Data.Dtos
{
    // DTO (Data Transfer Object) usado para transferir os dados necessários para o login do usuário.
    public class LoginUsuarioDto
    {
        // Define que este campo é obrigatório.
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        public string Username { get; set; }

        // Define que este campo é obrigatório.
        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; }
    }
}
