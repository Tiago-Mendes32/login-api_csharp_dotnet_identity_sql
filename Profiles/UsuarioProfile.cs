using AutoMapper;                // Importa o AutoMapper, que permite a conversão automática entre objetos.
using UsuariosApi.Data.Dtos;     // Importa os DTOs usados na transferência de dados.
using UsuariosApi.Models;        // Importa o modelo de usuário.

namespace UsuariosApi.Profiles
{
    // Classe de perfil do AutoMapper que define mapeamentos entre DTOs e modelos de domínio.
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            // Define um mapeamento automático entre CreateUsuarioDto e Usuario.
            CreateMap<CreateUsuarioDto, Usuario>();
        }
    }
}
