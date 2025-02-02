using System.Threading.Tasks;             // Importa o namespace para trabalhar com tarefas assíncronas.
using Microsoft.AspNetCore.Mvc;           // Importa o namespace que fornece recursos para construir controladores MVC.
using UsuariosApi.Data.Dtos;              // Importa os DTOs (Data Transfer Objects) usados para transferir dados.
using UsuariosApi.Services;               // Importa os serviços que contêm a lógica de negócio para operações com usuários.

namespace UsuariosApi.Controllers
{
    // Indica que esta classe é um controlador de API, que gerencia requisições HTTP.
    [ApiController]
    // Define a rota base para as ações deste controlador. "[Controller]" será substituído pelo nome da classe (UsuarioController).
    [Route("[Controller]")]
    public class UsuarioController : ControllerBase
    {
        // Declaração do serviço de usuário que será utilizado para executar as operações de negócio.
        private UsuarioService _usuarioService;

        // Construtor do controlador, que recebe uma instância de UsuarioService por meio de injeção de dependência.
        public UsuarioController(UsuarioService cadastroService)
        {
            _usuarioService = cadastroService;  // Atribui o serviço recebido à variável privada.
        }

        // Define uma ação HTTP POST para a rota "cadastro".
        // Essa ação é responsável por cadastrar um novo usuário.
        [HttpPost("cadastro")]
        public async Task<IActionResult> CadastraUsuario(CreateUsuarioDto dto)
        {
            // Chama o método do serviço para cadastrar o usuário, passando o DTO que contém os dados necessários.
            await _usuarioService.CadastraUsuario(dto);
            // Retorna uma resposta HTTP 200 (OK) com uma mensagem de sucesso.
            return Ok("Usuário cadastrado!");
        }

        // Define uma ação HTTP POST para a rota "login".
        // Essa ação é responsável por autenticar um usuário e gerar um token de acesso.
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginUsuarioDto dto)
        {
            // Chama o método de login do serviço, passando o DTO que contém as credenciais do usuário.
            // O método retorna um token caso a autenticação seja bem-sucedida.
            var token = await _usuarioService.Login(dto);
            // Retorna uma resposta HTTP 200 (OK) com o token gerado.
            return Ok(token);
        }
    }
}
