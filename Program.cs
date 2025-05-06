using Sabor_Brasil.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ConexaoService>();

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permite qualquer origem
              .AllowAnyMethod()  // Permite qualquer método HTTP (GET, POST, etc)
              .AllowAnyHeader(); // Permite qualquer cabeçalho
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var testar = scope.ServiceProvider.GetRequiredService<ConexaoService>();
    testar.TestarConexao();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();  // Essa linha garante que o servidor seja iniciado