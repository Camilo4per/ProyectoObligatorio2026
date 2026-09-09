# AGENTS.md — Proyecto Obligatorio (DDD + ASP.NET Core MVC)

## Overview
Sistema de facultad (DDD, 5 proyectos, `net10.0`) de **historias interactivas**: usuarios, historias, capítulos (Intermedio/Final) con opciones de destino y lecturas. Arquitectura en capas: `Dominio` (entidades + ValueObjects) → `DTOs`/`LogicaAplicacion` (casos de uso) → `LogicaAccesoDatos` (repositorios en memoria) → `Presentacion` (ASP.NET Core MVC). **No hay WebApi ni EF Core todavía**; la persistencia es en memoria tras interfaces.

```
ProyectoObligatorio/Dominio.csproj            (base, sin dependencias externas)
DTOs/DTOs.csproj                              → Dominio
LogicaAplicacion/LogicaAplicacion.csproj      → DTOs + Dominio
LogicaAccesoDatos/LogicaAccesoDatos.csproj    → Dominio
Presentacion/Presentacion.csproj (SDK Web)    → Dominio + DTOs + LogicaAplicacion + LogicaAccesoDatos
Solucion: ProyectoObligatorio.slnx
```

## Build & Run
```powershell
dotnet build                                        # compila la solucion .slnx (0 errores; ~34 warnings CS8618 esperados)
dotnet run --project Presentacion                   # levanta la app MVC (http localhost:5xxx)
dotnet build E:\CODIGO\Repos\Obligatorio\ProyectoObligatorio.slnx   # build explicito
```

## Testing
- `dotnet test` — **no hay proyectos de test en el repo**. No inventar ni ejecutar suites inexistentes.
- Si se agrega un proyecto xUnit/NUnit bajo `tests/`, ejecutar desde la raíz: `dotnet test`. Validar siempre con `dotnet build` tras cambios.

## Database & EF Core
- **EF Core NO está instalado**: no hay `DbContext`, no hay migraciones, no hay paquete `Microsoft.EntityFrameworkCore`. El acceso a datos vive en `LogicaAccesoDatos/RepositorioEnMemoria/` (patrón: interfaz en `Dominio/InterfaceDeRepositorio/` + implementación con `IRepository<T>`: `Add`, `Update`, `Remove`, `FindById`, `FindAll`).
- Los `Add()` llaman a `Entidad.Validar()` (IValidable). `Update`/`Remove` de los 6 repositorios tiran `NotImplementedException`.
- Cuando se migre a EF, los repositorios se reimplementan tras las MISMAS interfaces (el dominio no puede depender de EF). Comandos futuros (requieren el paquete de EF):
```powershell
dotnet ef migrations add NombreMigracion --project <capa-datos> --startup-project Presentacion
dotnet ef database update
```

## Code Style & Conventions
- **Modelo DDD estricto**: entidades = estado + reglas (`Validar()` lanza excepción de contexto: `UsuarioException`, `CapituloException`, etc.); ValueObjects = `record` validadores con propiedad `Valor`; `DTOs` = tipos planos (`string`/`int`, enums de dominio como `int`); `Mappers` = `static FromDTO`/`ToDTO`, **sin repositorios ni lógica de negocio**.
- **`CapituloMapper`** es el punto único: `FromDTO` infiere Final/Intermedio por `Opciones.Count == 0`, `ToDTO` distingue con `is Final`. `HistoriaMapper`/`OpcionMapper`/`LecturaMapper` delegan en él. `Capitulo` es abstracto; `Historia` tiene `CapituloInicial` + `List<Capitulo> Capitulos` (nada de `Pertenece`).
- **Controllers MVC** (en `Presentacion/Controllers/`): acciones delgadas `return View()`, inyección por constructor de **interfaces de casos de uso** (`IAgregarUsuario`, etc.), nunca instanciar repositorios. Registro DI SOLO en `Presentacion/Program.cs` (`builder.Services.AddScoped<...>`); los CU de Usuarios siguen comentados ahí.
- Nombres: clases/métodos en **PascalCase**, vars PascalCase en controllers (estilo actual del repo), archivos = nombre de la clase. `namespace` por capa (`Dominio.*`, `DTOs.DTOs`, `DTOs.Mappers`, `LogicaAplicacion.*`, `LogicaAccesoDatos.*`, `Presentacion.*`).
- Enums de dominio (`EstadoHistoria`, `RolesUsuario`) viajan como `int` en DTOs y se castean en el Mapper.
- Vistas Razor: usar `@using Presentacion.Models`, TagHelpers MVC, Bootstrap (bootstrap.min.css), refs a `~/lib`, `~/css`, `~/js`. Layout `_Layout.cshtml` ya usa `Context.Session` para Login/Rol.

## Constraints
- **NUNCA** meter lógica de negocio en Vistas Razor ni en Controladores: orquestar casos de uso, el dominio valida. Validar dos veces o castear DUPLICA reglas.
- **NUNCA** instanciar mappers/repositorios `new` fuera de su capa; todo pasa por DI. No duplicar endpoints: los controllers son la única interfaz HTTP (no hay WebApi; no crear una sin pedirlo).
- **NUNCA** agregar dependencias externas (EF, paquetes) en `Dominio`; no saltarse el `IRepository<T>` (un futuro DbContext va detrás de la misma interfaz, sin tocar dominio).
- **NUNCA** poner entidades de dominio dentro de DTOs (referencias y enums prohibidas; los `Mappers` hacen la conversión).
- **NUNCA** crear `IntermedioDTO`/`FinalDTO` polimórficos ni reintroducir `Pertenece`: el modelo es `CapituloDTO` único + inferencia de tipo en `CapituloMapper`.
- **NUNCA** llamar a EF/migraciones: no existe. Cambios de datos = `Add`/`Find` de `RepositorioEnMemoria`.
- No agregar `using` redundantes ni comentarios de relleno (repo limpio de ambos).

## Pendientes (verificados 07/09/2026)
- [ ] `Update`/`Remove` de los 6 repositorios en memoria (tiran `NotImplementedException`)
- [ ] Más casos de uso: `ModificarUsuario`, `EliminarUsuario` + CU para Historia/Capitulo/Opción/Categoría
- [ ] Flujo de `Lectura`: solo falta la capa de presentación (controller + vistas + DI en `Program.cs`)
- [ ] Corregir CS8618: 36 warnings, propiedades no-nullable sin inicializar
- [ ] No exponer `Password.Value` en `UsuarioMapper.ToDTO` (fallo de seguridad; **no prioritario**, decisión del docente)
- [ ] Revisar semántica de los `Existe*` (declarados `void`, lanzan excepción si existe; no devuelven `bool`)
- [ ] `HistoriaMapper.FromDTO`: ligar `CapDestino` por referencia/Id a las instancias de `Capitulos` (hoy copias) + regla "no compartir capítulos" en `RepositorioHistorias` con `Capitulos.Intersect(...)`
- [ ] Vistas y acciones para Usuario/Historia/Categoria/Capitulo (stubs con `return View()` sin vistas)

---
*Última actualización: 07/09/2026*