using System;                                // Importa funcionalidades básicas do .NET, como DateTime.
using Microsoft.IdentityModel.Tokens;       // Importa classes para geração e validação de tokens JWT.
using System.IdentityModel.Tokens.Jwt;      // Manipulação de tokens JWT.
using System.Security.Claims;               // Manipulação de claims (informações de identidade do usuário).
using System.Text;                           // Necessário para codificação da chave de segurança.
using UsuariosApi.Models;                    // Importa a classe Usuario.

namespace UsuariosApi.Services
{
    // Serviço responsável por gerar tokens JWT para autenticação de usuários.
    public class TokenService
    {
        private IConfiguration _configuration; // Armazena as configurações da aplicação, incluindo a chave de segurança.

        // Construtor que recebe a configuração da aplicação via injeção de dependência.
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método responsável por gerar um token JWT para um usuário autenticado.
        public string GenerateToken(Usuario usuario)
        {
            // Define as informações (claims) que serão incluídas no token.
            Claim[] claims = new Claim[]
            {
                new Claim("username", usuario.UserName), // Nome de usuário.
                new Claim("id", usuario.Id), // ID do usuário.
                new Claim(ClaimTypes.DateOfBirth, usuario.DataNascimento.ToString()), // Data de nascimento.
                new Claim("loginTimestamp", DateTime.UtcNow.ToString()) // Data e hora do login.
            };

            // Obtém a chave de segurança simétrica a partir das configurações.
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SymmetricSecurityKey"]));

            // Define as credenciais de assinatura usando a chave de segurança e o algoritmo HMAC SHA-256.
            var signingCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            // Cria o token JWT com as claims, a chave de assinatura e um tempo de expiração de 10 minutos.
            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(10), // Expiração do token.
                claims: claims, // Informações incluídas no token.
                signingCredentials: signingCredentials // Credenciais de assinatura.
            );

            // Converte o token para string e o retorna.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
