using Dominio.InterfaceDeRepositorio;
using LogicaAccesoDatos.RepositorioEntityFramework;
using LogicaAccesoDatos.RepositorioEntityFramework.Repositorios;
using LogicaAplicacion.CasosDeUso.Auditorias;
using LogicaAplicacion.CasosDeUso.Capitulos;
using LogicaAplicacion.CasosDeUso.Categorias;
using LogicaAplicacion.CasosDeUso.Historias;
using LogicaAplicacion.CasosDeUso.Lecturas;
using LogicaAplicacion.CasosDeUso.Usuarios;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Capitulos;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

//REGISTRAMOS EL CONTEXTO DE LA BASE DE DATOS
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
builder.Services.AddScoped<IRepositorioAuditorias, RepositorioAuditoriasEF>();

//INICIALIZAMOS LOS CASOS DE USO

builder.Services.AddScoped<IAgregarHistoria, AgregarHistoriaCU>();
builder.Services.AddScoped<IEncontrarTodasLasHistorias, EncontrarTodasLasHistoriasCU>();
builder.Services.AddScoped<IEncontrarHistoriasPublicadas, EncontrarHistoriasPublicadasCU>();
builder.Services.AddScoped<IObtenerHistoriaPorId, ObtenerHistoriaPorIdCU>();
builder.Services.AddScoped<IModificarHistoria, ModificarHistoriaCU>();
builder.Services.AddScoped<IEliminarHistoria, EliminarHistoriaCU>();

builder.Services.AddScoped<IEncontrarTodasCategorias, EncontrarTodasLasCategoriasCU>();

builder.Services.AddScoped<IAgregarUsuario, AgregarUsuarioCU>();
builder.Services.AddScoped<IEncontrarTodosUsuarios, EncontrarTodosUsuariosCU>();
builder.Services.AddScoped<IObtenerUsuarioPorId, ObtenerUsuarioPorIdCU>();
builder.Services.AddScoped<ILoginCU, LoginCU>();

builder.Services.AddScoped<IAgregarLectura, AgregarLecturaCU>();
builder.Services.AddScoped<IActualizarLectura, ActualizarLecturaCU>();
builder.Services.AddScoped<IObtenerLecturasPorId, ObtenerLecturasPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasLasLecturas, EncontrarTodasLasLecturasCU>();
builder.Services.AddScoped<IObtenerPasoLectura, ObtenerPasoLecturaCU>();
builder.Services.AddScoped<IAvanzarLectura, AvanzarLecturaCU>();

builder.Services.AddScoped<IAgregarCapitulo, AgregarCapituloCU>();
builder.Services.AddScoped<IRegistrarAuditoria, RegistrarAuditoriaCU>();
builder.Services.AddScoped<IConsultarAuditorias, ConsultarAuditoriasCU>();

// JWT Authentication
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
