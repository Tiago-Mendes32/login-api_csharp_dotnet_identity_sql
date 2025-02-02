using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Importa a classe base para IdentityDbContext.
using Microsoft.EntityFrameworkCore;                    // Importa funcionalidades do Entity Framework Core.
using UsuariosApi.Models;                               // Importa o modelo de usuário personalizado.

namespace UsuariosApi.Data
{
    // Contexto do banco de dados que gerencia usuários, estendendo IdentityDbContext para suporte à autenticação.
    public class UsuarioDbContext : IdentityDbContext<Usuario>
    {
        // Construtor que recebe opções de configuração do banco de dados e as passa para a classe base.
        public UsuarioDbContext(DbContextOptions<UsuarioDbContext> opts) : base(opts) { }
    }
}
