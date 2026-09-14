# AGENTS.md — Proyecto Obligatorio (DDD + ASP.NET Core MVC)

## Overview
Sistema de facultad (DDD, 5 proyectos, `net10.0`) de **historias interactivas**: usuarios, historias, capítulos (Intermedio/Final) con opciones de destino y lecturas. Arquitectura en capas: `Dominio` (entidades + ValueObjects) → `DTOs`/`LogicaAplicacion` (casos de uso) → `LogicaAccesoDatos` (repositorios en memoria + EF Core) → `Presentacion` (ASP.NET Core MVC). **No hay WebApi**. La persistencia hoy es en memoria tras interfaces; EF Core está parcialmente configurado (`HistoriasContext` + 1 migración) pero **no registrado en DI**.

```
ProyectoObligatorio/Dominio.csproj            (Microsoft.EntityFrameworkCore.Abstractions)
DTOs/DTOs.csproj                              → Dominio
LogicaAplicacion/LogicaAplicacion.csproj      → DTOs + Dominio
LogicaAccesoDatos/LogicaAccesoDatos.csproj    → Dominio (Microsoft.EntityFrameworkCore.SqlServer)
Presentacion/Presentacion.csproj (SDK Web)    → DTOs + LogicaAplicacion + LogicaAccesoDatos (Microsoft.EntityFrameworkCore.Tools)
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
- **EF Core está instalado pero no conectado**: `Dominio` tiene `Microsoft.EntityFrameworkCore.Abstractions` (atributos `[Owned]` en ValueObjects), `LogicaAccesoDatos` tiene `Microsoft.EntityFrameworkCore.SqlServer` con `HistoriasContext.cs` + 1 migración (`Inicial`), `Presentacion` tiene `Microsoft.EntityFrameworkCore.Tools`.
- **`HistoriasContext` NO está registrado en DI** (`Program.cs` no lo incluye). La app usa repositorios en memoria.
- La migración `Inicial` está **incompleta**: no mapea `Opciones` de `Intermedio`, el discriminador `nvarchar(8)` no alcanza para el nombre `Intermedio` (10 chars), falta configuración de doble FK en `Capitulos`.
- `Opcion` no tiene `Id` y no tendrá repositorio propio — se persiste con su `Capitulo`.
- `Update`/`Remove` de los 6 repositorios en memoria tiran `NotImplementedException`.
- Cuando se complete la migración a EF:
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
- `Registro.cshtml` **no pide rol**: `RegistrarUsuarioCU` sobreescribe `dto.Rol = (int)RolesUsuario.Usuario` para que nadie se registre como administrador.
- Vistas Razor: usar `@using Presentacion.Models`, TagHelpers MVC, Bootstrap (bootstrap.min.css), refs a `~/lib`, `~/css`, `~/js`. Layout `_Layout.cshtml` ya usa `Context.Session` para Login/Rol.

## Constraints
- **NUNCA** meter lógica de negocio en Vistas Razor ni en Controladores: orquestar casos de uso, el dominio valida. Validar dos veces o castear DUPLICA reglas.
- **NUNCA** instanciar mappers/repositorios `new` fuera de su capa; todo pasa por DI. No duplicar endpoints: los controllers son la única interfaz HTTP (no hay WebApi; no crear una sin pedirlo).
- **NUNCA** agregar dependencias externas (paquetes NuGet) en `Dominio` que no sean `Microsoft.EntityFrameworkCore.Abstractions`; no saltarse el `IRepository<T>` (un futuro DbContext va detrás de la misma interfaz, sin tocar dominio).
- **NUNCA** poner entidades de dominio dentro de DTOs (referencias y enums prohibidas; los `Mappers` hacen la conversión).
- **NUNCA** crear `IntermedioDTO`/`FinalDTO` polimórficos ni reintroducir `Pertenece`: el modelo es `CapituloDTO` único + inferencia de tipo en `CapituloMapper`.
- **NUNCA** crear `IRepository<Opcion>` ni repositorio de opciones: `Opcion` no tiene `Id` y se persiste embebida en `Intermedio`.
- No agregar `using` redundantes ni comentarios de relleno (repo limpio de ambos).

## Pendientes (verificados 10/09/2026)
- [ ] `Update`/`Remove` de los 6 repositorios en memoria (tiran `NotImplementedException`)
- [ ] Más casos de uso: `ModificarUsuario`, `EliminarUsuario` + CU para Historia/Capitulo/Categoría
- [ ] Flujo de `Lectura`: no hay controller ni vistas; registrar DI de lecturas en `Program.cs`
- [ ] Revisar semántica de los `Existe*` (declarados `void`, lanzan excepción si existe; no devuelven `bool`)
- [ ] `HistoriaMapper.FromDTO`: ligar `CapDestino` por referencia/Id a las instancias de `Capitulos` (hoy copias) + regla "no compartir capítulos" en `RepositorioHistorias` con `Capitulos.Intersect(...)`
- [ ] Vistas para acciones stub: Histórias/Categorias/Capítulos + `Modificar`/`Delete` de Usuario (solo existen vistas de Usuario: Index, Login, Registro)
- [ ] Completar migración EF: mapear `Opcion` embebida en `Intermedio`, fix discriminador TPH (`nvarchar(8)` vs `Intermedio` 10 chars), configurar doble FK en `Capitulos`, registrar `HistoriasContext` en DI
- [ ] `Opcion.Validar()` llama `Texto.Validar()` antes del null-check → NRE si `Texto` es null
- [ ] `Usuario.Validar()` llama `Password.Validar()`/`Email.Validar()`/`NombreCompleto.Validar()` sin null-checks → NRE si alguno es null
- [ ] `CapituloMapper.FromDTO` asume `dto.Opciones` no null → NPE si viene de form con Final
- [ ] `LecturaMapper.FromDTO` cast ciego `(Final) CapituloMapper.FromDTO(...)` → falla si el DTO tiene opciones
- [ ] Repositorios en memoria no asignan `Id` en `Add()` → múltiples entidades con `Id=0`
- [ ] `FindAll()` devuelve shallow copy pero entidades dentro son referencias compartidas → mutación entre callers
---
*Última actualización: 12/09/2026*
