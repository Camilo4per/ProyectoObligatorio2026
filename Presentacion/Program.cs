using Dominio.InterfaceDeRepositorio;
using LogicaAccesoDatos.RepositorioEnMemoria;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//INICIALIZAMOS LOS REPOSITORIOS
builder.Services.AddScoped<IRepositorioCategorias, RepositorioCategorias>();
builder.Services.AddScoped<IRepositorioFinales, RepositorioFinales>();
builder.Services.AddScoped<IRepositorioHistorias, RepositorioHistorias>();
builder.Services.AddScoped<IRepositorioIntermedios, RepositorioIntermedios>();
builder.Services.AddScoped<IRepositorioLecturas, RepositorioLecturas>();
builder.Services.AddScoped<IRepositorioOpciones, RepositorioOpciones>();
builder.Services.AddScoped<IRepositorioUsuarios, RepositorioUsuarios>();

//INICIALIZAMOS LOS CASOS DE USO


// builder.Services.AddScoped<IAgregarUsuario, AgregarUsuarioCU>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
