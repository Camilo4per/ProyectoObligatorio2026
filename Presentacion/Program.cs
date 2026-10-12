using Dominio.InterfaceDeRepositorio;
using Microsoft.EntityFrameworkCore;
using LogicaAccesoDatos.RepositorioEntityFramework;
using LogicaAccesoDatos.RepositorioEntityFramework.Repositorios;
using LogicaAplicacion.CasosDeUso.Usuarios;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using LogicaAplicacion.CasosDeUso.Historias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using LogicaAplicacion.CasosDeUso.Categorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDbContext<DominioContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ObligatorioDB"))
    );

//INICIALIZAMOS LOS REPOSITORIOS
builder.Services.AddScoped<IRepositorioCategorias, RepositorioCategoriasEF>();
builder.Services.AddScoped<IRepositorioFinales, RepositorioFinalesEF>();
builder.Services.AddScoped<IRepositorioHistorias, RepositorioHistoriasEF>();
builder.Services.AddScoped<IRepositorioIntermedios, RepositorioIntermediosEF>();
builder.Services.AddScoped<IRepositorioLecturas, RepositorioLecturasEF>();
builder.Services.AddScoped<IRepositorioUsuarios, RepositorioUsuariosEF>();

//INICIALIZAMOS LOS CASOS DE USO

builder.Services.AddScoped<IAgregarHistoria, AgregarHistoriaCU>();
builder.Services.AddScoped<IEncontrarTodasHistorias, EncontrarTodasLasHistoriasCU>();
builder.Services.AddScoped<IObtenerHistoriaPorId, ObtenerHistoriaPorIdCU>();
builder.Services.AddScoped<IModificarHistoria, ModificarHistoriaCU>();
builder.Services.AddScoped<IEliminarHistoria, EliminarHistoriaCU>();

builder.Services.AddScoped<IEncontrarTodasCategorias, EncontrarTodasLasCategoriasCU>();

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
