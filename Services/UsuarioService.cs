using System; // Importa funcionalidades básicas do .NET.
using System.Linq; // Permite usar LINQ, útil para manipulação de coleções.
using System.Threading.Tasks; // Permite o uso de operações assíncronas.
using AutoMapper; // Importa o AutoMapper para mapear objetos entre DTOs e modelos.
using Microsoft.AspNetCore.Identity; // Importa classes para autenticação e gerenciamento de identidade.
using UsuariosApi.Data.Dtos; // Importa os DTOs usados para transferência de dados.
using UsuariosApi.Models; // Importa o modelo de usuário.

namespace UsuariosApi.Services
{
    // Serviço responsável pelas operações de usuário, como cadastro e login.
    public class UsuarioService
    {
        // Dependências do serviço.
        private IMapper _mapper; // Usado para mapear entre DTO e modelo de domínio.
        private UserManager<Usuario> _userManager; // Gerencia as operações de usuário, como criação e manipulação.
        private SignInManager<Usuario> _signInManager; // Gerencia a autenticação de usuários.
        private TokenService _tokenService; // Serviço para gerar tokens JWT.

        // Construtor que recebe as dependências necessárias.
        public UsuarioService(IMapper mapper, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, TokenService tokenService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        // Método assíncrono para cadastrar um novo usuário.
        public async Task CadastraUsuario(CreateUsuarioDto dto)
        {
            // Mapeia o DTO para o modelo de usuário.
            Usuario usuario = _mapper.Map<Usuario>(dto);

            // Tenta criar o usuário no sistema com a senha fornecida.
            IdentityResult resultado = await _userManager.CreateAsync(usuario, dto.Password);

            // Se a criação falhar, lança uma exceção.
            if (!resultado.Succeeded)
            {
                throw new ApplicationException("Falha ao cadastrar usuário!");
            }
        }

        // Método assíncrono para realizar o login de um usuário.
        public async Task<string> Login(LoginUsuarioDto dto)
        {
            // Tenta autenticar o usuário com o nome de usuário e senha fornecidos.
            var resultado = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);

            // Se a autenticação falhar, lança uma exceção.
            if (!resultado.Succeeded)
            {
                throw new ApplicationException("Usuário não autenticado!");
            }

            // Após autenticar, busca o usuário correspondente.
            var usuario = _signInManager
                .UserManager
                .Users
                .FirstOrDefault(user => user.NormalizedUserName == dto.Username.ToUpper());

            // Gera o token JWT para o usuário autenticado.
            var token = _tokenService.GenerateToken(usuario);

            // Retorna o token gerado.
            return token;
        }
    }
}
