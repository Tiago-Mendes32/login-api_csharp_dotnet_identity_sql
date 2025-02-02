using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UsuariosApi.Authorization
{
    // Classe responsável por implementar a lógica de autorização baseada na idade mínima do usuário.
    public class IdadeAuthorization : AuthorizationHandler<IdadeMinima>
    {
        // Método que verifica se o usuário atende ao requisito de idade mínima.
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IdadeMinima requirement)
        {
            // Obtém a claim de data de nascimento do usuário autenticado.
            var dataNascimentoClaim = context
                .User.FindFirst(claim =>
                claim.Type == ClaimTypes.DateOfBirth); // ClaimTypes.DateOfBirth representa a data de nascimento do usuário.

            // Se a claim de data de nascimento não for encontrada, encerra a verificação.
            if (dataNascimentoClaim is null)
                return Task.CompletedTask;

            // Converte o valor da claim (que é uma string) para um objeto DateTime.
            var dataNascimento = Convert
                .ToDateTime(dataNascimentoClaim.Value);

            // Calcula a idade do usuário com base no ano atual e no ano da data de nascimento.
            var idadeUsuario =
                DateTime.Today.Year - dataNascimento.Year;

            // Ajusta a idade caso o usuário ainda não tenha feito aniversário neste ano.
            if (dataNascimento >
                DateTime.Today.AddYears(-idadeUsuario))
                idadeUsuario--;

            // Se a idade do usuário for maior ou igual à exigida, a autorização é concedida.
            if (idadeUsuario >= requirement.Idade)
                context.Succeed(requirement);

            // Finaliza a tarefa assincronamente.
            return Task.CompletedTask;
        }
    }
}
