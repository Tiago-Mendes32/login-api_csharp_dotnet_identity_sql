using Microsoft.AspNetCore.Authentication.JwtBearer;   // Importa classes necess�rias para autentica��o JWT.
using Microsoft.AspNetCore.Authorization;                // Importa classes para autoriza��o de usu�rios.
using Microsoft.AspNetCore.Identity;                    // Importa classes relacionadas � identidade e autentica��o.
using Microsoft.EntityFrameworkCore;                     // Importa Entity Framework Core, necess�rio para trabalhar com bancos de dados.
using Microsoft.IdentityModel.Tokens;                   // Importa classes para manipula��o de tokens JWT.
using System.Text;                                      // Usado para trabalhar com strings codificadas em UTF-8.
using UsuariosApi.Authorization;                        // Importa os namespaces relacionados � autoriza��o customizada.
using UsuariosApi.Data;                                 // Importa o namespace para o banco de dados.
using UsuariosApi.Models;                               // Importa o modelo de usu�rio.
using UsuariosApi.Services;                             // Importa os servi�os necess�rios.

var builder = WebApplication.CreateBuilder(args); // Cria o builder para configurar os servi�os e o pipeline da aplica��o.

// Adiciona os servi�os necess�rios ao container de depend�ncias.

var connString = builder.Configuration["ConnectionStrings:UsuarioConnection"]; // L� a string de conex�o para o banco de dados.

builder.Services.AddDbContext<UsuarioDbContext>(opts => // Configura o DbContext para usar o banco de dados.
    opts.UseMySql(connString, ServerVersion.AutoDetect(connString)) // Configura o banco MySQL, detectando a vers�o automaticamente.
);

// Configura o ASP.NET Identity para gerenciar a identidade dos usu�rios.
builder.Services
    .AddIdentity<Usuario, IdentityRole>() // Usa a classe `Usuario` personalizada e a classe `IdentityRole` para as permiss�es de usu�rio.
    .AddEntityFrameworkStores<UsuarioDbContext>() // Configura o Entity Framework como o reposit�rio de dados.
    .AddDefaultTokenProviders(); // Adiciona provedores de token padr�o para recupera��o de senha, etc.

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Configura o AutoMapper para mapear entre DTOs e modelos.

builder.Services.AddSingleton<IAuthorizationHandler, IdadeAuthorization>(); // Registra o handler de autoriza��o personalizado (IdadeMinima) como singleton.

builder.Services.AddControllers(); // Configura os controladores da API.
builder.Services.AddEndpointsApiExplorer(); // Configura a explora��o de endpoints para o Swagger.
builder.Services.AddSwaggerGen(); // Configura o Swagger para gerar a documenta��o da API.

builder.Services.AddAuthentication(options => // Configura a autentica��o para a aplica��o.
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Define o esquema de autentica��o como JWT.
})
.AddJwtBearer(options => // Configura��es espec�ficas para autentica��o via JWT.
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SymmetricSecurityKey"])), // Define a chave para assinatura do token.
        ValidateAudience = false, // N�o valida a audi�ncia (para esse caso, n�o � necess�rio).
        ValidateIssuer = false, // N�o valida o emissor (poderia ser configurado para uma aplica��o de maior seguran�a).
        ClockSkew = TimeSpan.Zero // Define o intervalo de aceita��o de tempo de expira��o do token.
    };
});

builder.Services.AddAuthorization(options => // Configura as pol�ticas de autoriza��o.
{
    options.AddPolicy("IdadeMinima", policy =>
         policy.AddRequirements(new IdadeMinima(18)) // Define uma pol�tica personalizada que exige que o usu�rio tenha, no m�nimo, 18 anos.
    );
});

builder.Services.AddScoped<UsuarioService>(); // Registra o servi�o `UsuarioService` como Scoped, ou seja, uma inst�ncia por requisi��o.
builder.Services.AddScoped<TokenService>(); // Registra o servi�o `TokenService` como Scoped.

var app = builder.Build(); // Constr�i a aplica��o a partir das configura��es acima.

// Configura o pipeline de requisi��o HTTP da aplica��o.

if (app.Environment.IsDevelopment()) // Se o ambiente for de desenvolvimento, ativa o Swagger.
{
    app.UseSwagger(); // Gera o Swagger.
    app.UseSwaggerUI(); // Interface do Swagger para explorar a API.
}

app.UseHttpsRedirection(); // Redireciona todas as requisi��es HTTP para HTTPS.
app.UseAuthentication(); // Ativa a autentica��o, usando o middleware JWT.
app.UseAuthorization(); // Ativa a autoriza��o, verificando se o usu�rio tem permiss�es adequadas.

app.MapControllers(); // Mapeia os controladores para lidar com as requisi��es HTTP.

app.Run(); // Inicia a aplica��o.
