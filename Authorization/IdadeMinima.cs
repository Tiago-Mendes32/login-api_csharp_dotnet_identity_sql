using Microsoft.AspNetCore.Authorization;

namespace UsuariosApi.Authorization
{
    // Define um requisito de autorização para a idade mínima do usuário.
    // Essa classe será usada pelo manipulador de autorização para validar se o usuário atende ao critério de idade mínima.
    public class IdadeMinima : IAuthorizationRequirement
    {
        // Construtor que recebe a idade mínima exigida como parâmetro.
        public IdadeMinima(int idade)
        {
            Idade = idade;
        }

        // Propriedade que armazena a idade mínima necessária para autorização.
        public int Idade { get; set; }
    }
}
