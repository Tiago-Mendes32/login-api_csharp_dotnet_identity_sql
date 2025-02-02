using Microsoft.AspNetCore.Authentication.JwtBearer;   // Importa classes necessárias para autenticação JWT.
using Microsoft.AspNetCore.Authorization;                // Importa classes para autorização de usuários.
using Microsoft.AspNetCore.Identity;                    // Importa classes relacionadas à identidade e autenticação.
using Microsoft.EntityFrameworkCore;                     // Importa Entity Framework Core, necessário para trabalhar com bancos de dados.
using Microsoft.IdentityModel.Tokens;                   // Importa classes para manipulação de tokens JWT.
using System.Text;                                      // Usado para trabalhar com strings codificadas em UTF-8.
using UsuariosApi.Authorization;                        // Importa os namespaces relacionados à autorização customizada.
using UsuariosApi.Data;                                 // Importa o namespace para o banco de dados.
using UsuariosApi.Models;                               // Importa o modelo de usuário.
using UsuariosApi.Services;                             // Importa os serviços necessários.

var builder = WebApplication.CreateBuilder(args); // Cria o builder para configurar os serviços e o pipeline da aplicação.

// Adiciona os serviços necessários ao container de dependências.

var connString = builder.Configuration["ConnectionStrings:UsuarioConnection"]; // Lê a string de conexão para o banco de dados.

builder.Services.AddDbContext<UsuarioDbContext>(opts => // Configura o DbContext para usar o banco de dados.
    opts.UseMySql(connString, ServerVersion.AutoDetect(connString)) // Configura o banco MySQL, detectando a versão automaticamente.
);

// Configura o ASP.NET Identity para gerenciar a identidade dos usuários.
builder.Services
    .AddIdentity<Usuario, IdentityRole>() // Usa a classe `Usuario` personalizada e a classe `IdentityRole` para as permissões de usuário.
    .AddEntityFrameworkStores<UsuarioDbContext>() // Configura o Entity Framework como o repositório de dados.
    .AddDefaultTokenProviders(); // Adiciona provedores de token padrão para recuperação de senha, etc.

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Configura o AutoMapper para mapear entre DTOs e modelos.

builder.Services.AddSingleton<IAuthorizationHandler, IdadeAuthorization>(); // Registra o handler de autorização personalizado (IdadeMinima) como singleton.

builder.Services.AddControllers(); // Configura os controladores da API.
builder.Services.AddEndpointsApiExplorer(); // Configura a exploração de endpoints para o Swagger.
builder.Services.AddSwaggerGen(); // Configura o Swagger para gerar a documentação da API.

builder.Services.AddAuthentication(options => // Configura a autenticação para a aplicação.
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Define o esquema de autenticação como JWT.
})
.AddJwtBearer(options => // Configurações específicas para autenticação via JWT.
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SymmetricSecurityKey"])), // Define a chave para assinatura do token.
        ValidateAudience = false, // Não valida a audiência (para esse caso, não é necessário).
        ValidateIssuer = false, // Não valida o emissor (poderia ser configurado para uma aplicação de maior segurança).
        ClockSkew = TimeSpan.Zero // Define o intervalo de aceitação de tempo de expiração do token.
    };
});

builder.Services.AddAuthorization(options => // Configura as políticas de autorização.
{
    options.AddPolicy("IdadeMinima", policy =>
         policy.AddRequirements(new IdadeMinima(18)) // Define uma política personalizada que exige que o usuário tenha, no mínimo, 18 anos.
    );
});

builder.Services.AddScoped<UsuarioService>(); // Registra o serviço `UsuarioService` como Scoped, ou seja, uma instância por requisição.
builder.Services.AddScoped<TokenService>(); // Registra o serviço `TokenService` como Scoped.

var app = builder.Build(); // Constrói a aplicação a partir das configurações acima.

// Configura o pipeline de requisição HTTP da aplicação.

if (app.Environment.IsDevelopment()) // Se o ambiente for de desenvolvimento, ativa o Swagger.
{
    app.UseSwagger(); // Gera o Swagger.
    app.UseSwaggerUI(); // Interface do Swagger para explorar a API.
}

app.UseHttpsRedirection(); // Redireciona todas as requisições HTTP para HTTPS.
app.UseAuthentication(); // Ativa a autenticação, usando o middleware JWT.
app.UseAuthorization(); // Ativa a autorização, verificando se o usuário tem permissões adequadas.

app.MapControllers(); // Mapeia os controladores para lidar com as requisições HTTP.

app.Run(); // Inicia a aplicação.
