using Dominio.InterfaceDeRepositorio;
using LogicaAccesoDatos.RepositorioEntityFramework;
using LogicaAccesoDatos.RepositorioEntityFramework.Repositorios;
using LogicaAplicacion.CasosDeUso.Categorias;
using LogicaAplicacion.CasosDeUso.Historias;
using LogicaAplicacion.CasosDeUso.Usuarios;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

//REGISTRAMOS EL CONTEXTO QUE USAN LOS REPOSITORIOS EF
builder.Services.AddDbContext<DominioContext>();

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

builder.Services.AddScoped<IRegistrarUsuario, RegistrarUsuarioCU>();
builder.Services.AddScoped<IEncontrarTodosUsuarios, EncontrarTodosUsuariosCU>();
builder.Services.AddScoped<IObtenerUsuarioPorId, ObtenerUsuarioPorIdCU>();
builder.Services.AddScoped<ILoginUsuario, LoginUsuarioCU>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
