using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsuariosApi.Controllers
{
    // Indica que esta classe é um controlador de API no ASP.NET Core.
    [ApiController]
    [Route("[Controller]")] // Define a rota base da API como o nome do controlador (AcessoController).
    public class AcessoController : ControllerBase
    {
        // Define um endpoint HTTP GET acessível na rota "/Acesso".
        [HttpGet]
        // Aplica a política de autorização "IdadeMinima", exigindo que o usuário atenda a esse requisito para acessar este método.
        [Authorize(Policy = "IdadeMinima")]
        public IActionResult Get()
        {
            // Retorna uma resposta HTTP 200 (OK) com a mensagem "Acesso permitido!".
            return Ok("Acesso permitido!");
        }
    }
}
