# AGENTS.md — Proyecto Obligatorio (DDD + ASP.NET Core MVC)

## Overview
Sistema de facultad (DDD, 5 proyectos, `net10.0`) de **historias interactivas**: usuarios, historias, capítulos (Intermedio/Final) con opciones de destino y lecturas. Arquitectura en capas: `Dominio` (entidades + ValueObjects) → `DTOs`/`LogicaAplicacion` (casos de uso) → `LogicaAccesoDatos` (repositorios EF Core + en memoria) → `Presentacion` (ASP.NET Core MVC). **No hay WebApi**. La persistencia activa es **EF Core/SQL Server**: los 6 repos registrados en DI (`IRepositorioCategorias`, `Finales`, `Historias`, `Intermedios`, `Lecturas`, `Usuarios`) son los `Repositorio*EF` sobre `DominioContext` (registrado con `AddDbContext`). Los `Repositorio*` de `LogicaAccesoDatos/RepositorioEnMemoria` siguen existiendo pero **ya no están en DI**.

```
ProyectoObligatorio/Dominio.csproj            (Microsoft.EntityFrameworkCore.Abstractions)
DTOs/DTOs.csproj                              → Dominio
LogicaAplicacion/LogicaAplicacion.csproj      → DTOs + Dominio
LogicaAccesoDatos/LogicaAccesoDatos.csproj    → Dominio (Microsoft.EntityFrameworkCore.SqlServer 10.0.12)
Presentacion/Presentacion.csproj (SDK Web)    → DTOs + LogicaAplicacion + LogicaAccesoDatos + Dominio (Microsoft.EntityFrameworkCore.Tools)
Solucion: ProyectoObligatorio.slnx
```

## Build & Run
```powershell
dotnet build                                        # compila la solucion .slnx (0 errores, 0 avisos)
dotnet run --project Presentacion                   # levanta la app MVC (http localhost:5018)
```
- La app arranca con DI registrado en `Program.cs`: `builder.Services.AddDbContext<DominioContext>()` (sin opciones, la cadena sale de `OnConfiguring`) y los 6 `IRepositorio*` → `Repositorio*EF` (SQL Server, base local `ObligatorioDB`). Los repos en memoria NO están registrados.
- **`dotnet ef` NO está instalado en el shell del agente** (y no instalar herramientas sin autorización). Para operaciones de DB (drop/update) usar un proyecto temporal que referencie `LogicaAccesoDatos` y llame `context.Database.EnsureDeleted()`/`Migrate()`, o pedirle al usuario que corra `dotnet ef`.

## Testing
- `dotnet test` — **no hay proyectos de test en el repo**. No inventar ni ejecutar suites inexistentes.
- Si se agrega un proyecto xUnit/NUnit bajo `tests/`, ejecutar desde la raíz: `dotnet test`. Validar siempre con `dotnet build` tras cambios.

## Database & EF Core
- **EF Core 10.0.12**: `Dominio` tiene `Microsoft.EntityFrameworkCore.Abstractions` (atributos `[Owned]` en ValueObjects), `LogicaAccesoDatos` tiene `Microsoft.EntityFrameworkCore.SqlServer` con `DominioContext.cs` + migraciones, `Presentacion` tiene `Microsoft.EntityFrameworkCore.Tools`.
- **`DominioContext.cs`**: `DbSet`s Usuarios, Categorias, Historias, Capitulos, Intermedios, Finales, Lecturas (+ `DbSet<Opcion> Opciones`, un owned type que admite el contexto — innecesario pero inofensivo). Conexión **hardcodeada** en `OnConfiguring`: `SERVER=(localdb)\MsSqlLocalDb;DATABASE=ObligatorioDB;Integrated Security=true;`. No se lee de `appsettings.json`. **Está registrado en DI**: `AddDbContext<DominioContext>()` en `Program.cs` sin opciones, de modo que `OnConfiguring` provee la cadena.
- **`OnModelCreating` centraliza las relaciones que las anotaciones no pueden**:
  - FK única `Capitulos.idHistoria → Historias` con **`OnDelete(Cascade)`** (borrar una historia borra sus capítulos). El shadow `HistoriaId` ya **no existe** (antes se duplicaba con `idHistoria`).
  - `Historias.idCapituloInicial → Capitulos` queda **NoAction** (dos cascadas entre las mismas tablas = error "múltiples rutas" en SQL Server).
  - `Opcion` (`[Owned]`, **sin `Id`**, mapeada con `OwnsMany` desde `Intermedio.Opciones`) → tabla **`Opcion`** con PK `(idCapitulo, Id)`, `idCapDestino → Capitulos` (NoAction) e `idCapitulo → Capitulos` (Cascade). `Opcion` usa `string Texto` (se eliminó el VO `TextoOpcion`).
  - **Categorias ↔ Historias es M:N** con join table implícita **`CategoriaHistoria`** (PK `(CategoriasId, HistoriasId)`, FKs en `Cascade`), vía `Categoria.Historias` + `Historia.Categorias`. `Categorias` ya **no** tiene `HistoriaId`.
  - `Lecturas.idFinal → Capitulos.Id` con FK única (anotación apuntando a `FinalAlcanzado`).
- **Migraciones: solo 2, regeneradas desde cero** (se borraron `CambiosEF`, `cambioLectura`, `cambioOpciones/2`, `cambiosOpciones` y las `Inicial` viejas):
  - `20260915220706_Inicial` — esquema completo (TPH `Capitulos` con `Discriminator nvarchar(13)`, todas las tablas y FKs).
  - `20260915223410_AgregacionDeCategoriaHistorias` — M:N: dropea `Categorias.HistoriaId` y crea `CategoriaHistoria`.
- La **base local fue recreada limpia** (drop + update) y está 1:1 con el modelo y estas 2 migraciones (historial sin migraciones fantasma).
- `Update`/`Remove` de los 6 repos EF ya están **implementados** (ver "Completado el 17/09/2026"); los 6 `Repositorio*` de `RepositorioEnMemoria` siguen con `NotImplementedException` pero **ya no están en DI**.
- Para nuevas migraciones:
```powershell
dotnet ef migrations add NombreMigracion --project LogicaAccesoDatos --startup-project Presentacion
dotnet ef database update
```

## Code Style & Conventions
- **Modelo DDD estricto**: entidades = estado + reglas (`Validar()` lanza excepción de contexto: `UsuarioException`, `CapituloException`, etc.); ValueObjects = `record` validadores con propiedad `Valor`; `DTOs` = tipos planos (`string`/`int`, enums de dominio como `int`); `Mappers` = `static FromDTO`/`ToDTO`, **sin repositorios ni lógica de negocio**.
- **`CapituloMapper`** es el punto único: `FromDTO` infiere Final/Intermedio por `Opciones.Count == 0`, `ToDTO` distingue con `is Final`. `HistoriaMapper`/`OpcionMapper`/`LecturaMapper` delegan en él. `Capitulo` es abstracto; `Historia` tiene `CapituloInicial` + `List<Capitulo> Capitulos` (nada de `Pertenece`).
- **Controllers MVC** (en `Presentacion/Controllers/`): acciones delgadas `return View()`, inyección por constructor de **interfaces de casos de uso** (`IAgregarUsuario`, etc.), nunca instanciar repositorios. Registro DI SOLO en `Presentacion/Program.cs` (`builder.Services.AddScoped<...>`); los CU de Usuarios ya están registrados y activos.
- Nombres: clases/métodos en **PascalCase**, vars PascalCase en controllers (estilo actual del repo), archivos = nombre de la clase. `namespace` por capa (`Dominio.*`, `DTOs.DTOs`, `DTOs.Mappers`, `LogicaAplicacion.*`, `LogicaAccesoDatos.*`, `Presentacion.*`).
- Enums de dominio (`EstadoHistoria`, `RolesUsuario`) viajan como `int` en DTOs y se castean en el Mapper.
- `Registro.cshtml` **pide rol** (opciones hardcodeadas `Administrador (0)` / `Usuario (1)`, coinciden con `RolesUsuario`); `RegistrarUsuarioCU` ya **no** sobreescribe `dto.Rol` — antes se forzaba `RolesUsuario.Usuario` para que nadie se registrara como administrador (ver "Completado el 17/09/2026").
- Vistas Razor: usar `@using Presentacion.Models`, TagHelpers MVC, Bootstrap (bootstrap.min.css), refs a `~/lib`, `~/css`, `~/js`. Layout `_Layout.cshtml` usa `Context.Session`: claves **`IdUsuario`**, **`Rol`** y **`NombreUsuario`** (login las setea en `UsuarioController`; el footer las muestra — antes usaba `LogeadoRol`/`LogeadoNombre`/`LogeadoCedula` que no existían).

## Constraints
- **NUNCA** meter lógica de negocio en Vistas Razor ni en Controladores: orquestar casos de uso, el dominio valida. Validar dos veces o castear DUPLICA reglas.
- **NUNCA** instanciar mappers/repositorios `new` fuera de su capa; todo pasa por DI. No duplicar endpoints: los controllers son la única interfaz HTTP (no hay WebApi; no crear una sin pedirlo).
- **NUNCA** agregar dependencias externas (paquetes NuGet) en `Dominio` que no sean `Microsoft.EntityFrameworkCore.Abstractions`; no saltarse el `IRepository<T>` (un futuro DbContext va detrás de la misma interfaz, sin tocar dominio).
- **NUNCA** poner entidades de dominio dentro de DTOs (referencias y enums prohibidas; los `Mappers` hacen la conversión).
- **NUNCA** crear `IntermedioDTO`/`FinalDTO` polimórficos ni reintroducir `Pertenece`: el modelo es `CapituloDTO` único + inferencia de tipo en `CapituloMapper`.
- **NUNCA** crear `IRepository<Opcion>` ni repositorio de opciones: `Opcion` no tiene `Id` y se persiste embebida en su capítulo (tabla `Opcion` vía `OwnsMany`).
- Las anotaciones `[Owned]`/`[ForeignKey]` en las entidades **se conservan por pedido del profe** aunque alguna (ej. `Opcion.idCapitulo` → `Intermedio`) no tenga navegación que respalde: la relación real la define el Fluent de `OnModelCreating`.
- No agregar `using` redundantes ni comentarios de relleno (repo limpio de ambos).

## Pendientes (verificados 17/09/2026)
- [ ] **Login roto (bug nuevo)**: `Views/Usuario/Login.cshtml` postea inputs `name="email"`/`name="password"` pero la acción es `Login(string usuario, string password)` → `usuario` nunca se bindea y el login siempre falla. Renombrar el input a `usuario` (o el parametro a `email`). Nota: `LoginUsuarioCU` compara por `NombreUsuario`, no por email
- [ ] **Registrarse como Administrador**: tras el cambio de hoy la vista lo permite y `RegistrarUsuarioCU` ya no sobreescribe `Rol`. Decidir si se mantiene o se vuelve a forzar `RolesUsuario.Usuario` en el CU
- [ ] Más casos de uso: `ModificarUsuario`, `EliminarUsuario` + CU para Historia/Capitulo/Categoría
- [ ] Flujo de `Lectura`: no hay controller ni vistas; registrar DI de lecturas en `Program.cs`
- [ ] Revisar semántica de los `Existe*` (declarados `void`, lanzan excepción si existe; no devuelven `bool`)
- [ ] `HistoriaMapper.FromDTO`: ligar `CapDestino` por referencia/Id a las instancias de `Capitulos` (hoy copias) + regla "no compartir capítulos" en `RepositorioHistorias` con `Capitulos.Intersect(...)`
- [ ] Vistas stub que faltan: Historias/Categorias/Capítulos completas + `Detalles`/`Modificar`/`Delete` de Usuario (solo existen vistas de Usuario: Index, Login, Registro; los GET `Modificar`/`Delete` actuales tiran "view not found")
- [ ] `Usuario.Validar()` llama `Password.Validar()`/`Email.Validar()`/`NombreCompleto.Validar()` sin null-checks → NRE si alguno es null
- [ ] `CapituloMapper.FromDTO` asume `dto.Opciones` no null → NPE si viene de form con Final
- [ ] `LecturaMapper.FromDTO` cast ciego `(Final) CapituloMapper.FromDTO(...)` → falla si el DTO tiene opciones
- [ ] Repositorios en memoria no asignan `Id` en `Add()` → múltiples entidades con `Id=0`
- [ ] `FindAll()` devuelve shallow copy pero entidades dentro son referencias compartidas → mutación entre callers
- [ ] `DiagramaDeClases.asta.lock` quedó untracked (artefacto de la herramienta ASTA; conviene agregar a `.gitignore` y decidir si el `.asta` modificado se commitea)

## Completado el 17/09/2026 (verificado con `dotnet build`)
- [x] **Persistencia activa pasada a EF/SQL Server**: `Program.cs` registra `AddDbContext<DominioContext>()` (sin opciones → `OnConfiguring` provee la cadena) y los 6 `IRepositorio*` ahora resuelven a `Repositorio*EF` (Categorias, Finales, Historias, Intermedios, Lecturas, Usuarios). `Presentacion.csproj` agrega `ProjectReference` a `..\ProyectoObligatorio\Dominio.csproj`. Los repos en memoria quedaron fuera de DI.
- [x] **Creados los 5 repos EF faltantes** (`RepositorioCategoriasEF`, `RepositorioFinalesEF`, `RepositorioHistoriasEF`, `RepositorioIntermediosEF`, `RepositorioLecturasEF`) y actualizado `RepositorioUsuariosEF`: todos reciben el `DominioContext` **inyectado** por constructor (se eliminó el `new DominioContext()`). Patrón uniforme: `Add` valida + `Existe*` + `SaveChanges`; `Update` valida + `Update` + `SaveChanges`; `Remove` borra con `Remove(new Entidad { Id = id })` + `SaveChanges` (Finales/Intermedios usan `_context.Capitulos.Where(...).ExecuteDelete()`); `FindById` usa `FirstOrDefault() ?? throw <Entidad>Exception("... inexistente")` (ya no devuelve null); `FindAll` = `ToList()`.
- [x] **`Update`/`Remove` implementados en los 6 repos EF** (resuelto el pendiente de `NotImplementedException`, que solo queda en los repos en memoria fuera de DI).
- [x] **`IRepositorioUsuarios` vuelve a depender de EF**: con el contexto por DI, la app ya no usa el repositorio en memoria para Usuarios (resuelto el pendiente del constructor + `FindById` null).
- [x] **`Registro.cshtml` pide rol de nuevo**: se quitó `asp-items="ViewBag.Roles"` y las opciones se hardcodean (`Administrador (0)` / `Usuario (1)`, coinciden con `RolesUsuario` donde `Administrador=0`); se corrigió el typo `S` en el `form-group` del Email. `RegistrarUsuarioCU` ya no sobreescribe `Rol`, así que hoy es posible registrarse como Administrador (ver pendiente "Registrarse como Administrador").

## Completado el 15/09/2026 (verificado con `dotnet build` 0/0 + arranque real de la app)
- [x] **FK duplicada en `Lecturas`** (bug de `idFinal`): `[ForeignKey(nameof(Final))]` apuntaba a una navegación inexistente → la columna quedaba huérfana y EF creaba `FinalAlcanzadoId` shadow. Corregido a `[ForeignKey(nameof(FinalAlcanzado))]`; `LecturaMapper` mapea `idFinal`; hoy la tabla tiene solo `idFinal → Capitulos.Id`.
- [x] **Doble FK en `Capitulos`** (`HistoriaId` shadow + `idHistoria` huérfana): resuelto con Fluent `Entity<Capitulo>().HasOne<Historia>().WithMany(h => h.Capitulos).HasForeignKey(c => c.idHistoria)`; la columna shadow ya no se genera.
- [x] **Borrado en cascada Historia → Capitulos** (era la pregunta al profe): implementado con `.OnDelete(DeleteBehavior.Cascade)` en la FK `idHistoria`. `Historias.idCapituloInicial` quedó NoAction para no chocar con la regla de "múltiples rutas de cascada" de SQL Server.
- [x] **`Opcion` mapeada a tabla propia** (entidad débil, sin `Id`): `OwnsMany` en `OnModelCreating` → tabla `Opcion` (PK `(idCapitulo, Id)`), `Intermedio.Opciones` pasó de campo a propiedad `{ get; set; }`, `Texto` de `string` (se eliminó `TextoOpcion`), `OpcionDTO`/`OpcionMapper` con `idCapitulo`/`idCapDestino`.
- [x] **Categorias ↔ Historias M:N**: agregada `Categoria.Historias`; join table implícita `CategoriaHistoria`; `Categorias` sin `HistoriaId`.
- [x] **Migraciones y base limpias**: se regeneraron desde cero 2 migraciones (`20260915220706_Inicial` + `20260915223410_AgregacionDeCategoriaHistorias`), se eliminaron todas las previas (parches a mano/PHps etiquetas viejas), y la base local se recreó (drop + update) 1:1 con el modelo.
- [x] **Crash de arranque por DI**: `IRepositorioUsuarios` pasó de `RepositorioUsuariosEF` → `RepositorioUsuarios` (en memoria), eliminando la dependencia de `DominioContext` sin registrar. La app arranca (localhost:5018).
- [x] **Login/sesión**: `UsuarioController` setea `NombreUsuario` en Session; `_Layout.cshtml` usa las claves reales (`IdUsuario`, `Rol`, `NombreUsuario`) en vez de `LogeadoRol`/`LogeadoNombre`/`LogeadoCedula`.
- [x] **`Opcion.Validar()`** ya valida null-safe (`Texto` null-check antes de usarlo).
---
*Última actualización: 17/09/2026*