using Dominio.InterfaceDeRepositorio;
using LogicaAccesoDatos.RepositorioEnMemoria;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using LogicaAccesoDatos.EF;
using Microsoft.EntityFrameworkCore;

using LogicaAplicacion.CasosDeUso.Usuarios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

//INICIALIZAMOS LOS REPOSITORIOS
builder.Services.AddScoped<IRepositorioCategorias, RepositorioCategorias>();
builder.Services.AddScoped<IRepositorioFinales, RepositorioFinales>();
builder.Services.AddScoped<IRepositorioHistorias, RepositorioHistorias>();
builder.Services.AddScoped<IRepositorioIntermedios, RepositorioIntermedios>();
builder.Services.AddScoped<IRepositorioLecturas, RepositorioLecturas>();
builder.Services.AddScoped<IRepositorioUsuarios, RepositorioUsuarios>();

//INICIALIZAMOS LOS CASOS DE USO
builder.Services.AddScoped<IAgregarUsuario, AgregarUsuarioCU>();
builder.Services.AddScoped<IEncontrarTodosUsuarios, EncontrarTodosUsuariosCU>();
builder.Services.AddScoped<IObtenerUsuarioPorId, ObtenerUsuarioPorIdCU>();
builder.Services.AddScoped<ILoginUsuario, LoginUsuarioCU>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Usuario/Index");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
