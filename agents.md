# AGENTS.md — Proyecto Obligatorio (DDD + ASP.NET Core MVC)

## Overview
Sistema de facultad (DDD, 5 proyectos, `net10.0`) de **historias interactivas**: usuarios, historias, capítulos (Intermedio/Final) con opciones de destino y lecturas. Arquitectura en capas: `Dominio` (entidades + ValueObjects) → `DTOs`/`LogicaAplicacion` (casos de uso) → `LogicaAccesoDatos` (repositorios EF Core + en memoria) → `Presentacion` (ASP.NET Core MVC). **No hay WebApi**. La persistencia activa es **EF Core/SQL Server**: los 6 repos registrados en DI (`IRepositorioCategorias`, `Finales`, `Historias`, `Intermedios`, `Lecturas`, `Usuarios`) son los `Repositorio*EF` sobre `DominioContext` (registrado con `AddDbContext`). Los `Repositorio*` de `LogicaAccesoDatos/RepositorioEnMemoria` siguen existiendo pero **ya no están en DI**.

Estado funcional (verificado 24/09/2026): usuarios (registro/login/lista), **CRUD de historias completo** (alta, edición, borrado, detalles, listado con categorías), categorías (solo listado). Capítulos y Lecturas: sin CU ni vistas aún. Categoría/Capítulo tienen controller **stub** (scaffold sin vistas).

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
dotnet build                                        # compila la solucion .slnx (0 errores; los avisos CS8618/CS8625 pre-existentes solo son visibles en rebuild completo)
dotnet run --project Presentacion                   # levanta la app MVC (http localhost:5018)
```
- La app arranca con DI registrado en `Program.cs`: `builder.Services.AddDbContext<DominioContext>()` (sin opciones, la cadena sale de `OnConfiguring`) y los 6 `IRepositorio*` → `Repositorio*EF` (SQL Server, base local `ObligatorioDB`). Los repos en memoria NO están registrados.
- Casos de uso registrados: Historias (`IAgregarHistoria`, `IEncontrarTodasHistorias`, `IObtenerHistoriaPorId`, `IModificarHistoria`, `IEliminarHistoria`), Categorías (`IEncontrarTodasCategorias`), Usuarios (`IRegistrarUsuario`, `IEncontrarTodosUsuarios`, `IObtenerUsuarioPorId`, `ILoginUsuario`).
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
- La **base local fue recreada limpia** (drop + update) y está 1:1 con el modelo y estas 2 migraciones (historial sin migraciones fantasma). Contiene las 3 categorías de producción: Aventura (1), Terror (2), Ciencia Ficción (3).
- Para nuevas migraciones:
```powershell
dotnet ef migrations add NombreMigracion --project LogicaAccesoDatos --startup-project Presentacion
dotnet ef database update
```

### Patrones de `RepositorioHistoriasEF` (NO cambiar sin motivo)
- **`Add`: guardado en dos fases** por la dependencia circular Historias ↔ CapituloInicial: romper el ciclo con `CapituloInicial = null!`, `Add` + `SaveChanges`, restaurar la navegación y persistir `idCapituloInicial` en un **segundo `SaveChanges`**. Sin transacción (decisión explícita del usuario).
- **`Update`: patrón mutar-trackeado** (no `Update(grafoNuevo)`): carga la entidad con `Include(Capitulos/Categorias)`, muta `Titulo`/`Sinopsis`/`Estado`, reconcilia `Categorias` contra instancias trackeadas, y `SaveChanges`. Re-adjuntar un grafo nuevo desde el DTO chocaba con el tracking ("misma key ya trackeada") y re-insertaba joins de `CategoriaHistoria` (PK violation). El flujo de Edit de la vista no edita capítulos, así que `Update` no los toca.
- **Nota de EF**: las colecciones `OwnsMany` vacías (ej. `Intermedio.Opciones`) se materializan como **lista vacía, no `null`**. Esto condiciona la inferencia de `CapituloMapper` (ver Code Style).

## Code Style & Conventions
- **Modelo DDD estricto**: entidades = estado + reglas (`Validar()` lanza excepción de contexto: `UsuarioException`, `CapituloException`, etc.); ValueObjects = `record` validadores con propiedad `Valor`; `DTOs` = tipos planos (`string`/`int`, enums de dominio como `int`); `Mappers` = `static FromDTO`/`ToDTO`, **sin repositorios ni lógica de negocio**.
- **`Capitulo` es abstracto** (TPH); `Historia` tiene `CapituloInicial` + `List<Capitulo> Capitulos` (nada de `Pertenece`). `Historia.Titulo`/`Sinopsis` son `get; set;` (pasaron de `init` a `set` para permitir el patrón mutar-trackeado de `Update`; las validaciones del dominio siguen). Los VOs viajan como `int` en DTOs (enums de dominio).
- **Round-trip de capítulos (`CapituloMapper` + `HistoriaMapper`)** — mantener la simetría `ToDTO`/`FromDTO`:
  - `CapituloMapper.FromDTO` infiere: `Opciones == null → Intermedio`, `Count == 0 → Final`, `con items → Intermedio`. Para que eso sea consistente, **`ToDTO` emite `Opciones = null` para un Intermedio sin opciones** y `new List<OpcionDTO>()` para un `Final` (si se serializara lista vacía, el Intermedio sin opciones round-trippea como Final → "el capítulo inicial no puede ser de tipo final").
  - `HistoriaMapper.ToDTO` **reutiliza el DTO del `CapituloInicial`** dentro de `Capitulos` (por `ReferenceEquals`) y `FromDTO` hace el espejo: así el round-trip no duplica el capítulo inicial (misma instancia → mismo DTO → misma entidad).
- **Controllers MVC** (en `Presentacion/Controllers/`): acciones delgadas `return View()`, inyección por constructor de **interfaces de casos de uso**, nunca instanciar repositorios. Registro DI SOLO en `Presentacion/Program.cs`. **`CategoriaController`/`CapituloController` son stubs de scaffold** (sin CU, `return View()` sin vistas) — no usarlos como referencia.
- Nombres: clases/métodos en **PascalCase**, vars PascalCase en controllers (estilo actual del repo), archivos = nombre de la clase. `namespace` por capa (`Dominio.*`, `DTOs.DTOs`, `DTOs.Mappers`, `LogicaAplicacion.*`, `LogicaAccesoDatos.*`, `Presentacion.*`).
- Vistas Razor: usar `@using Presentacion.Models`, TagHelpers MVC, Bootstrap (bootstrap.min.css), refs a `~/lib`, `~/css`, `~/js`. Layout `_Layout.cshtml` usa `Context.Session`: claves **`IdUsuario`**, **`Rol`** y **`NombreUsuario`** (login las setea en `UsuarioController`; el footer las muestra).
- **Vistas existentes**: Usuario (`Index`, `Login`, `Registro`), Historia (`Index`, `Create`, `Edit`, `Details`, `Delete`). Faltan Categoría, Capítulo y `Detalles`/`Modificar`/`Delete` de Usuario.

## Constraints
- **NUNCA** meter lógica de negocio en Vistas Razor ni en Controladores: orquestar casos de uso, el dominio valida. Validar dos veces o castear DUPLICA reglas.
- **NUNCA** instanciar mappers/repositorios `new` fuera de su capa; todo pasa por DI. No duplicar endpoints: los controllers son la única interfaz HTTP (no hay WebApi; no crear una sin pedirlo).
- **NUNCA** agregar dependencias externas (paquetes NuGet) en `Dominio` que no sean `Microsoft.EntityFrameworkCore.Abstractions`; no saltarse el `IRepository<T>` (un futuro DbContext va detrás de la misma interfaz, sin tocar dominio).
- **NUNCA** poner entidades de dominio dentro de DTOs (referencias y enums prohibidas; los `Mappers` hacen la conversión).
- **NUNCA** crear `IntermedioDTO`/`FinalDTO` polimórficos ni reintroducir `Pertenece`: el modelo es `CapituloDTO` único + inferencia de tipo en `CapituloMapper`.
- **NUNCA** crear `IRepository<Opcion>` ni repositorio de opciones: `Opcion` no tiene `Id` y se persiste embebida en su capítulo (tabla `Opcion` vía `OwnsMany`).
- Las anotaciones `[Owned]`/`[ForeignKey]` en las entidades **se conservan por pedido del profe** aunque alguna (ej. `Opcion.idCapitulo` → `Intermedio`) no tenga navegación que respalde: la relación real la define el Fluent de `OnModelCreating`.
- No agregar `using` redundantes ni comentarios de relleno (repo limpio de ambos).

## Pendientes (actualizados 24/09/2026)
- [ ] **Registrarse como Administrador**: la vista lo permite (opciones hardcodeadas `Administrador (0)` / `Usuario (1)`) y `RegistrarUsuarioCU` ya no sobreescribe `Rol`. Decidir si se mantiene o se vuelve a forzar `RolesUsuario.Usuario` en el CU
- [ ] **Password en texto plano**: `Password.Verify` compara `value == Value` sin hash (el propio código lo comenta: "sin hash por ahora"). Es la mejora de seguridad #1 del proyecto
- [ ] **`LoginUsuarioCU` trae todo a memoria**: recorre `repositorio.FindAll()` y compara en C#. Mejor un `FindByNombreUsuario` en el repo EF (query a la base)
- [ ] **POST Delete sin model**: en `UsuarioController` el catch hace `return View()` sin model pero `Delete.cshtml` usa `@Model` → NRE solo si el borrado falla. Pasar el model o redirigir (en `HistoriaController` ya está resuelto, ver Completado 24/09)
- [ ] **CUs que faltan**: `ModificarUsuario`/`EliminarUsuario`; CRUD de **Capítulo** (no existe ninguno); CRUD de **Categoría** (hoy solo `EncontrarTodasLasCategorias`). Historias ya tiene sus 5 CUs
- [ ] **`CategoriaController`/`CapituloController` son stubs**: no usan CUs y devuelven `View()` sin vistas → tiran "view not found". Faltan las vistas de Categoría y Capítulo, y de Usuario faltan `Detalles`/`Modificar`/`Delete` (solo existen `Index`, `Login`, `Registro`)
- [ ] Flujo de **`Lectura`**: no hay controller ni vistas; registrar DI de lecturas en `Program.cs`
- [ ] Revisar semántica de los `Existe*` (declarados `void`, lanzan excepción si existe; no devuelven `bool`)
- [ ] `OpcionMapper`/destinos: ligar `CapDestino` por referencia/Id a las instancias de `Capitulos` (hoy copias) + regla "no compartir capítulos" en `RepositorioHistorias` con `Capitulos.Intersect(...)` (el duplicado del inicial ya está resuelto)
- [ ] `Usuario.Validar()` llama `Password.Validar()`/`Email.Validar()`/`NombreCompleto.Validar()` sin null-checks → NRE si alguno es null
- [ ] `LecturaMapper.FromDTO` cast ciego `(Final) CapituloMapper.FromDTO(...)` → falla si el DTO tiene opciones
- [ ] Repositorios en memoria no asignan `Id` en `Add()` → múltiples entidades con `Id=0` (fuera de DI)
- [ ] `FindAll()` devuelve shallow copy pero entidades dentro son referencias compartidas → mutación entre callers
- [ ] `DiagramaDeClases.asta.lock` quedó untracked (artefacto de la herramienta ASTA; conviene agregar a `.gitignore` y decidir si el `.asta` modificado se commitea)

## Completado el 24/09/2026 (verificado con `dotnet build` 0/0 + pruebas contra la base real)
- [x] **Login verificado funcionando**: la vista ya postea `name="usuario"` (antes el pendiente decía `email`) y coincide con la acción `Login(string usuario, string password)`; `LoginUsuarioCU` valida `NombreUsuario` + `Password.Verify`.
- [x] **Categorías sembradas en la base**: Aventura (1), Terror (2), Ciencia Ficción (3) — insertadas con script temporal (proyecto eliminado) y verificadas.
- [x] **Alta de historia con dependencia circular resuelta**: `RepositorioHistoriasEF.Add` guarda en dos fases — rompe el ciclo con `CapituloInicial = null!`, `Add` + `SaveChanges`, restaura la navegación y persiste `idCapituloInicial` en un segundo `SaveChanges`. (Sin transacción: decisión explícita del usuario.)
- [x] **Capítulo duplicado en el alta**: `HistoriaMapper.FromDTO` reutiliza la instancia del inicial ya mapeada vía `ReferenceEquals` cuando `Capitulos` contiene el mismo DTO (`Capitulos.Count=1`, misma instancia en `CapituloInicial` y `Capitulos[0]`).
- [x] **Edición de historia arreglada (bug "no se permiten capítulos de tipo final")**: `CapituloMapper.ToDTO` emite `Opciones = null` para un Intermedio sin opciones, así la inferencia de `FromDTO` es consistente (`null → Intermedio`, `lista vacía → Final`, `con items → Intermedio`). Causa raíz: EF materializa la colección `OwnsMany` vacía como **lista vacía (no null)** y el round-trip convertía el Intermedio sin opciones en `Final`.
- [x] **Capítulo duplicado en el round-trip de Edit**: `HistoriaMapper.ToDTO` reutiliza el DTO del inicial (por `ReferenceEquals`) dentro de `Capitulos` (espejo del fix de `FromDTO`).
- [x] **`RepositorioHistoriasEF.Update` reescrito (patrón mutar-trackeado)**: carga la entidad con `Include(Capitulos/Categorias)`, muta `Titulo`/`Sinopsis`/`Estado` y reconcilia `Categorias` contra instancias trackeadas, y `SaveChanges`. Elimina los dos crashes del patrón anterior (`Update(grafoNuevo)`): "misma key ya trackeada" y violación de PK en `CategoriaHistoria`. Para poder mutar, `Historia.Titulo`/`Sinopsis` pasaron de `init` a `set` (las validaciones del dominio siguen activas).
- [x] Verificado end-to-end contra la base real (script temporal eliminado): crear → modificar (título/sinopsis/estado + cambio de categorías) → capítulo inicial intacto → borrar. Base dejada limpia.
- [x] **Baja de historia bloqueada si tiene lecturas (Opción A implementada)**: `EliminarHistoriaCU` inyecta `IRepositorioLecturas`, valida existencia con `FindById` y lanza `HistoriaException("No se puede eliminar la historia porque tiene lecturas asociadas. Debe eliminar las lecturas antes de darla de baja.")` si `TieneLecturasDeHistoria(id)` es true. El nuevo método cubre lecturas por `idHistoria` directa o por `idFinal → Capitulos.idHistoria` (una lectura terminada en un final de esa historia también la protege). Implementado en `IRepositorioLecturas`, `RepositorioLecturasEF` (EF) y `RepositorioLecturas` (en memoria, no DI).
- [x] **`RepositorioHistoriasEF.Remove` en SQL directo (tracker-free)**: ahora que el CU carga la historia con `FindById` antes de borrar, el change tracker no podía ordenar el borrado (FKs circulares `Capitulos.idHistoria` Cascade + `Historias.idCapituloInicial` NoAction → "circular dependency") y cascadaba lecturas trackeadas (DELETE de filas ya inexistentes → `DbUpdateConcurrencyException`). El `Remove` ahora es `_context.Historias.Where(h => h.Id == id).ExecuteDelete()`: una sola sentencia que se saltea al change tracker y la base resuelve la cascada (capítulos + joins de `CategoriaHistoria`); mismo patrón que Finales/Intermedios. Nota: `Lecturas.idHistoria` es FK opcional sin cascade (base en NO ACTION), así que una historia con lecturas tampoco se puede borrar por base si alguien esquiva el CU.
- [x] **POST Delete de `HistoriaController` ya no tira NRE**: el catch re-renderiza la confirmación con `obtenerHistoriaPorId.Ejecutar(id)` (antes `return View()` sin model y `Delete.cshtml` usa `@Model`), así el mensaje de error descriptivo se muestra en el cartel rojo.
- [x] **Opción A verificada end-to-end contra la base real** (script temporal eliminado): crear usuario + historia + lectura → baja bloqueada con el mensaje descriptivo e historia intacta → quitar la lectura → baja OK → limpieza completa. Base dejada limpia (0 lecturas, solo datos reales).
- [x] **RF03 Implementado Completo (Gestión de historias — Rol Administrador CRUD)**:
  - **Autorización y Menú**: `HistoriaController.OnActionExecuting` restringe todas las acciones (`Index`, `Create`, `Edit`, `Delete`, `Details`) exclusivamente a usuarios con rol `Administrador` (redirige a `Login` en caso contrario); `_Layout.cshtml` muestra el botón "Historias" solo si el rol en sesión es `Administrador`.
  - **Edición del capítulo inicial condicionado**: `ModificarHistoriaCU` evalúa si la historia ya contiene otros capítulos (`Capitulos.Count > 1`). Si los contiene e intenta modificarse el inicial, lanza `HistoriaException("El capítulo inicial no se puede modificar porque la historia ya posee otros capítulos agregados.")`. Si solo cuenta con el inicial (`Capitulos.Count <= 1`), actualiza título y texto del inicial, y `RepositorioHistoriasEF.Update` muta `existente.CapituloInicial` en base.
  - **Vista Edit adaptativa**: `Edit.cshtml` expone los campos de título y texto del capítulo inicial editables si `Capitulos.Count <= 1`, o como `readonly` con alerta informativa si `Capitulos.Count > 1`.
  - **Reglas de Publicación en el Dominio**: `Historia.ValidarPublicacion()` se invoca desde `Historia.Validar()` cuando `Estado == EstadoHistoria.Publicada`. Exige que la historia tenga al menos un capítulo `Final`, que todos los capítulos `Intermedio` tengan al menos una opción, y que todas las opciones apunten a otro capítulo válido dentro de la misma historia (intermedio o final).
  - **Carga de Opciones en Repositorio**: `RepositorioHistoriasEF.FindById` y `FindAll` cargan los capítulos intermedios con sus opciones (`OwnsMany`), asegurando que la validación de publicación y los DTOs dispongan del grafo completo de opciones.
  - **Verificado end-to-end contra la base real**: comprobada la restricción de modificación del capítulo inicial y el bloqueo/éxito de publicación con opciones y destinos. Base dejada limpia.

## Completado el 17/09/2026 (verificado con `dotnet build`)
- [x] **Persistencia activa pasada a EF/SQL Server**: `Program.cs` registra `AddDbContext<DominioContext>()` (sin opciones → `OnConfiguring` provee la cadena) y los 6 `IRepositorio*` ahora resuelven a `Repositorio*EF` (Categorias, Finales, Historias, Intermedios, Lecturas, Usuarios). `Presentacion.csproj` agrega `ProjectReference` a `..\ProyectoObligatorio\Dominio.csproj`. Los repos en memoria quedaron fuera de DI.
- [x] **Creados los 5 repos EF faltantes** (`RepositorioCategoriasEF`, `RepositorioFinalesEF`, `RepositorioHistoriasEF`, `RepositorioIntermediosEF`, `RepositorioLecturasEF`) y actualizado `RepositorioUsuariosEF`: todos reciben el `DominioContext` **inyectado** por constructor (se eliminó el `new DominioContext()`). Patrón uniforme: `Add` valida + `Existe*` + `SaveChanges`; `Update` valida + `Update` + `SaveChanges`; `Remove` borra con `Remove(new Entidad { Id = id })` + `SaveChanges` (Finales/Intermedios borran sus capítulos con `_context.Capitulos.Where(...).ExecuteDelete()`; Historias usa `_context.Historias.Where(...).ExecuteDelete()`, ver Completado 24/09); `FindById` usa `FirstOrDefault() ?? throw <Entidad>Exception("... inexistente")` (ya no devuelve null); `FindAll` = `ToList()`.
- [x] **`Update`/`Remove` implementados en los 6 repos EF** (resuelto el pendiente de `NotImplementedException`, que solo queda en los repos en memoria fuera de DI).
- [x] **`IRepositorioUsuarios` vuelve a depender de EF**: con el contexto por DI, la app ya no usa el repositorio en memoria para Usuarios (resuelto el pendiente del constructor + `FindById` null).
- [x] **`Registro.cshtml` pide rol de nuevo**: se quitó `asp-items="ViewBag.Roles"` y las opciones se hardcodean (`Administrador (0)` / `Usuario (1)`, coinciden con `RolesUsuario` donde `Administrador=0`); se corrigió el typo `S` en el `form-group` del Email. `RegistrarUsuarioCU` ya no sobreescribe `Rol`, así que hoy es posible registrarse como Administrador (ver pendiente "Registrarse como Administrador").

## Completado el 15/09/2026 (verificado con `dotnet build` 0/0 + arranque real de la app)
- [x] **FK duplicada en `Lecturas`** (bug de `idFinal`): `[ForeignKey(nameof(Final))]` apuntaba a una navegación inexistente → la columna quedaba huérfana y EF creaba `FinalAlcanzadoId` shadow. Corregido a `[ForeignKey(nameof(FinalAlcanzado))]`; `LecturaMapper` mapea `idFinal`; hoy la tabla tiene solo `idFinal → Capitulos.Id`.
- [x] **Doble FK en `Capitulos`** (`HistoriaId` shadow + `idHistoria` huérfana): resuelto con Fluent `Entity<Capitulo>().HasOne<Historia>().WithMany(h => h.Capitulos).HasForeignKey(c => c.idHistoria)`; la columna shadow ya no se genera.
- [x] **Borrado en cascada Historia → Capitulos**: implementado con `.OnDelete(DeleteBehavior.Cascade)` en la FK `idHistoria`. `Historias.idCapituloInicial` quedó NoAction para no chocar con la regla de "múltiples rutas de cascada" de SQL Server.
- [x] **`Opcion` mapeada a tabla propia** (entidad débil, sin `Id`): `OwnsMany` en `OnModelCreating` → tabla `Opcion` (PK `(idCapitulo, Id)`), `Intermedio.Opciones` pasó de campo a propiedad `{ get; set; }`, `Texto` de `string` (se eliminó `TextoOpcion`), `OpcionDTO`/`OpcionMapper` con `idCapitulo`/`idCapDestino`.
- [x] **Categorias ↔ Historias M:N**: agregada `Categoria.Historias`; join table implícita `CategoriaHistoria`; `Categorias` sin `HistoriaId`.
- [x] **Migraciones y base limpias**: se regeneraron desde cero 2 migraciones (`20260915220706_Inicial` + `20260915223410_AgregacionDeCategoriaHistorias`), se eliminaron todas las previas (parches a mano/PHps etiquetas viejas), y la base local se recreó (drop + update) 1:1 con el modelo.
- [x] **Crash de arranque por DI**: `IRepositorioUsuarios` pasó de `RepositorioUsuariosEF` → `RepositorioUsuarios` (en memoria), eliminando la dependencia de `DominioContext` sin registrar. La app arranca (localhost:5018). *(Estado intermedio: revertido el 17/09 al pasar todo a EF.)*
- [x] **Login/sesión**: `UsuarioController` setea `NombreUsuario` en Session; `_Layout.cshtml` usa las claves reales (`IdUsuario`, `Rol`, `NombreUsuario`) en vez de `LogeadoRol`/`LogeadoNombre`/`LogeadoCedula`.
- [x] **`Opcion.Validar()`** ya valida null-safe (`Texto` null-check antes de usarlo).
---
*Última actualización: 24/09/2026*