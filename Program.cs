using Sabor_Brasil.Services; // <- ESSENCIAL

var builder = WebApplication.CreateBuilder(args);

// Registra o serviço na injeção de dependência
builder.Services.AddScoped<UsuarioService>();

var app = builder.Build();

// Testa a conexão
using (var scope = app.Services.CreateScope())
{
    var usuarioService = scope.ServiceProvider.GetRequiredService<UsuarioService>();
    usuarioService.TestarConexao();
}
