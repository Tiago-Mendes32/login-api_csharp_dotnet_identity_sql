using System;                         // Importa funcionalidades básicas do .NET, como o tipo DateTime.
using Microsoft.AspNetCore.Identity;  // Importa a classe base IdentityUser, usada para autenticação.

namespace UsuariosApi.Models
{
    // Define a classe de usuário que herda de IdentityUser, permitindo personalização dos dados do usuário.
    public class Usuario : IdentityUser
    {
        // Propriedade adicional para armazenar a data de nascimento do usuário.
        public DateTime DataNascimento { get; set; }

        // Construtor padrão que chama o construtor da classe base IdentityUser.
        public Usuario() : base() { }
    }
}
