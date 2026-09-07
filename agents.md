# Documentación del Proyecto - Arquitectura DDD

## Descripción General
Proyecto de facultad utilizando arquitectura DDD (Domain-Driven Design). El sistema gestiona usuarios, historias interactivas y capítulos con opciones de destino.

## Arquitectura Actual
El proyecto sigue una arquitectura en capas con separación por proyectos (5 proyectos):

```
Dominio (Entidades + ValueObjects) ←→ DTOs ←→ LogicaAplicacion (Casos de Uso)
                                              ←→ LogicaAccesoDatos (Repositorios)
                                                      ↓
                                              Presentacion (Controladores MVC)
```

### Capas:

1. **Dominio** (`ProyectoObligatorio/`)
   - Entidades: `Usuario`, `Historia`, `Capitulo` (abstract), `Intermedio`, `Final`, `Opcion`, `Categoria`
   - Enums: `EstadoHistoria` (Borrador, Publicada), `RolesUsuario` (Administrador, Usuario)
   - ValueObjects: `Email`, `Password`, `NombreCompleto`, `Titulo`, `Sinopsis`, `Texto`, `Nombre`, `CantLlegaron`
   - Excepciones: `UsuarioException`, `CapituloException`, `CategoriaException`, `FinalException`, `HistoriaException`, `IntermedioException`, `OpcionException`
   - Interfaces: `IValidable` (internal)
   - InterfaceDeRepositorio: `IRepository<T>`, `IRepositorioUsuarios`, `IRepositorioHistorias`, `IRepositorioCategorias`, `IRepositorioOpcion`, `IRepositorioIntermedios`, `IRepositorioFinales`

2. **DTOs** (`DTOs/`)
   - DTOs (5): `UsuarioDTO`, `HistoriaDTO`, `CapituloDTO`, `CategoriaDTO`, `OpcionDTO`
   - Mappers (5): `UsuarioMapper`, `HistoriaMapper`, `CapituloMapper`, `CategoriaMapper`, `OpcionMapper`

3. **LogicaAplicacion** (`LogicaAplicacion/`)
   - Casos de uso de Usuarios (implementados): `AgregarUsuarioCU`, `EncontrarTodosUsuariosCU`, `ObtenerUsuarioPorIdCU`
   - Interfaces de casos de uso: `IAgregarUsuario`, `IEncontrarTodosUsuarios`, `IObtenerUsuarioPorId`
   - Nota: dar de alta CU en `Presentacion/Program.cs` (DI)

4. **LogicaAccesoDatos** (`LogicaAccesoDatos/`)
   - RepositorioEnMemoria: `RepositorioUsuarios`, `RepositorioHistorias`, `RepositorioCategorias`, `RepositorioOpcion`, `RepositorioIntermedios`, `RepositorioFinales` (parcialmente implementados)

5. **Presentacion** (`Presentacion/`) - capa MVC (ASP.NET Core)
   - Controllers: `UsuarioController`, `HistoriaController`, `CategoriaController`, `CapituloController` (todos con acciones stub)
   - **No existe HomeController** (la vista `Views/Home/Index.cshtml` no tiene controlador)
   - DI incompleta en `Program.cs` (solo registra `IRepositorioUsuarios`; los casos de uso no están registrados)

## Estructura de Archivos

### Dominio - Entidades
| Archivo | Descripción |
|---------|-------------|
| `Entidades/Usuario.cs` | Entidad con ValueObjects (NombreCompleto, Email, Password) y roles |
| `Entidades/Historia.cs` | Entidad con ValueObjects (Titulo, Sinopsis), listado de Categorias, `CapituloInicial` y `List<Capitulo> Capitulos` |
| `Entidades/Capitulo.cs` | Clase abstracta base para Intermedio y Final |
| `Entidades/Intermedio.cs` | Capítulo con lista de Opciones |
| `Entidades/Final.cs` | Capítulo final con contador de llegadas |
| `Entidades/Opcion.cs` | Opción con texto y capítulo destino |
| `Entidades/Categoria.cs` | Categoría con Id y Nombre |
| `Entidades/EstadoHistoria.cs` | Enum: Borrador, Publicada |
| `Entidades/RolesUsuario.cs` | Enum: Administrador, Usuario |

### Dominio - ValueObjects
| Archivo | Descripción |
|---------|-------------|
| `ValueObjects/Usuario/Email.cs` | Record con validación de formato (@, .) |
| `ValueObjects/Usuario/Password.cs` | Record con validación de fortaleza (8+ chars, mayús, minús, número, especial). ToString enmascara |
| `ValueObjects/Usuario/NombreCompleto.cs` | Record con Nombre y Apellido. ToString combina ambos |
| `ValueObjects/Historia/Titulo.cs` | Record con validación de longitud (3-100 chars) |
| `ValueObjects/Historia/Sinopsis.cs` | Record con validación de longitud (10-500 chars) |
| `ValueObjects/Capitulo/Titulo.cs` | Record con validación de longitud (3-100 chars). Propio del contexto Capitulo (no reutiliza el de Historia) |
| `ValueObjects/Capitulo/TextoCapitulo.cs` | Record con validación de longitud (3-2000 chars). Exclusivo del contexto Capitulo |
| `ValueObjects/Opcion/TextoOpcion.cs` | Record con validación de longitud (3-2000 chars). Exclusivo del contexto Opcion |
| `ValueObjects/Categoria/Nombre.cs` | Record con validación de longitud (3-50 chars) |
| `ValueObjects/Final/CantLlegaron.cs` | Record con validación no negativa (>= 0) |

### Dominio - Excepciones
| Archivo | Descripción |
|---------|-------------|
| `Excepciones/UsuarioException.cs` | Excepción para errores de usuario |
| `Excepciones/CapituloException.cs` | Excepción para errores de capítulo |
| `Excepciones/CategoriaException.cs` | Excepción para errores de categoría |
| `Excepciones/FinalException.cs` | Excepción para errores de capítulo final |
| `Excepciones/HistoriaException.cs` | Excepción para errores de historia |
| `Excepciones/IntermedioException.cs` | Excepción para errores de capítulo intermedio |
| `Excepciones/OpcionException.cs` | Excepción para errores de opción |

### Dominio - Repositorios
| Archivo | Descripción |
|---------|-------------|
| `InterfaceDeRepositorio/IRepository.cs` | Interfaz genérica: Add, Update, Remove(int id), FindById → T, FindAll → List<T> |
| `InterfaceDeRepositorio/IRepositorioUsuarios.cs` | Extiende IRepository<Usuario>, agrega ExisteUsuario |
| `InterfaceDeRepositorio/IRepositorioHistorias.cs` | Extiende IRepository<Historia>, agrega ExisteHistoria |
| `InterfaceDeRepositorio/IRepositorioCategorias.cs` | Extiende IRepository<Categoria>, agrega ExisteCategoria |
| `InterfaceDeRepositorio/IRepositorioOpcion.cs` | Extiende IRepository<Opcion>, agrega ExisteOpcion |
| `InterfaceDeRepositorio/IRepositorioIntermedios.cs` | Extiende IRepository<Intermedio>, agrega ExisteIntermedio |
| `InterfaceDeRepositorio/IRepositorioFinales.cs` | Extiende IRepository<Final>, agrega ExisteFinal |

### Dominio - Implementación IValidable
| Entidad | Estado de Validar() |
|---------|--------------------|
| `Usuario` | Implementada (valida NombreUsuario) |
| `Capitulo` | Implementada (`virtual`): valida Titulo y Texto. Base de Intermedio/Final |
| `Intermedio` | Implementada (`override`): valida titulo/texto heredado + al menos 1 opción |
| `Final` | Implementada (`override`): valida titulo/texto heredado + CantLlegaron |
| `Opcion` | Implementada: valida Texto y CapDestino (no nulo, evita ciclos A→B→A) |
| `Categoria` | Implementada: valida Nombre |
| `Historia` | Implementada: valida Titulo, Sinopsis, Categorias (no vacías) y Capitulo |
| `Lectura` | Implementada: valida Usuario, Historia y fechas (inicio previa a fin) |

> Los ValueObjects ya implementan `IValidable` (todos): Email, Password, NombreCompleto, Titulo (Historia/Capitulo), Sinopsis, TextoCapitulo, TextoOpcion, Nombre, CantLlegaron. Cada uno lanza la excepción de su contexto.

> **Estado:** todas las entidades implementan `Validar()`. Los `Add()` de los repositorios de Historia, Categoria, Intermedio, Final, Opcion y Lectura ya funcionan de punta a punta.

### DTOs
| Archivo | Descripción |
|---------|-------------|
| `DTOs/DTOs/UsuarioDTO.cs` | Transferencia de datos con Nombre y Apellido separados. `Rol` es `int` (se castea a `RolesUsuario` en el Mapper) |
| `DTOs/DTOs/HistoriaDTO.cs` | Transferencia de datos con Titulo y Sinopsis como strings. `CapituloInicial` + `List<CapituloDTO> Capitulos`. `Estado` es `int` (se castea a `EstadoHistoria` en el Mapper) |
| `DTOs/DTOs/CapituloDTO.cs` | DTO unificado de capítulo (Id, Titulo, Texto, CantLlegaron, Opciones). Integra a los viejos IntermedioDTO y FinalDTO |
| `DTOs/DTOs/CategoriaDTO.cs` | Transferencia de datos de categoría (Nombre) |
| `DTOs/DTOs/OpcionDTO.cs` | Transferencia de datos de opción (Id, Texto, CapDestino) |
| `DTOs/DTOs/LecturaDTO.cs` | Transferencia de datos de lectura con DTOs anidados (`UsuarioDTO`, `HistoriaDTO`, `CapituloDTO?` para el Final). Fechas `DateTime` planas (`FechaInicio`, `FechaFin?`)
| `DTOs/Mappers/UsuarioMapper.cs` | Conversión DTO → Entidad (FromDTO) y Entidad → DTO (ToDTO). Castea `int → RolesUsuario` |
| `DTOs/Mappers/HistoriaMapper.cs` | Conversión DTO → Entidad (FromDTO) y Entidad → DTO (ToDTO). Castea `int → EstadoHistoria`. Mapea `CapituloInicial` y `Capitulos` delegando en `CapituloMapper` |
| `DTOs/Mappers/CapituloMapper.cs` | Dos métodos únicos: `FromDTO` (→ `Capitulo`, infiere el tipo por `Opciones.Count == 0` → Final, si no Intermedio) y `ToDTO` (`Capitulo` → CapituloDTO, distingue con `is Final`). Copian el Id |
| `DTOs/Mappers/CategoriaMapper.cs` | Conversión DTO → Entidad (FromDTO) y Entidad → DTO (ToDTO) |
| `DTOs/Mappers/OpcionMapper.cs` | `FromDTO`/`ToDTO` simples: el `CapDestino` se delega en `CapituloMapper.FromDTO`/`ToDTO` (allá se infiere el tipo) |
| `DTOs/Mappers/LecturaMapper.cs` | `FromDTO`/`ToDTO` con DTOs anidados. Reutiliza `UsuarioMapper`, `HistoriaMapper` y `CapituloMapper` (sin repositorios) |

### LogicaAplicacion
| Archivo | Descripción |
|---------|-------------|
| `CasosDeUso/Usuarios/AgregarUsuarioCU.cs` | Implementado (Add + UsuarioMapper.FromDTO) |
| `CasosDeUso/Usuarios/EncontrarTodosLosUsuariosCU.cs` | Implementado (FindAll + ToDTO). Clase real: `EncontrarTodosUsuariosCU` |
| `CasosDeUso/Usuarios/ObtenerUsuarioPorIdCU.cs` | Implementado (FindById + ToDTO) |
| `InterfacesDeCasoDeUso/Usuarios/IAgregarUsuario.cs` | Interfaz: void Ejecutar(UsuarioDTO) |
| `InterfacesDeCasoDeUso/Usuarios/IEncontrarTodosLosUsuarios.cs` | Interfaz: List<UsuarioDTO> Ejecutar(). Interfaz real: `IEncontrarTodosUsuarios` |
| `InterfacesDeCasoDeUso/Usuarios/IObtenerUsuarioPorId.cs` | Interfaz: UsuarioDTO Ejecutar(int id) |
| `CasosDeUso/Lecturas/AgregarLecturaCU.cs` | Implementado (Add + LecturaMapper.FromDTO). Inyecta solo IRepositorioLecturas |
| `CasosDeUso/Lecturas/EncontrarTodasLecturasCU.cs` | Implementado (FindAll + ToDTO) |
| `CasosDeUso/Lecturas/ObtenerLecturaPorIdCU.cs` | Implementado (FindById + ToDTO) |
| `InterfacesDeCasoDeUso/Lecturas/IAgregarLectura.cs` | Interfaz: void Ejecutar(LecturaDTO) |
| `InterfacesDeCasoDeUso/Lecturas/IEncontrarTodasLecturas.cs` | Interfaz: List<LecturaDTO> Ejecutar() |
| `InterfacesDeCasoDeUso/Lecturas/IObtenerLecturaPorId.cs` | Interfaz: LecturaDTO Ejecutar(int id) |

### LogicaAccesoDatos
| Archivo | Descripción |
|---------|-------------|
| `RepositorioEnMemoria/RepositorioUsuarios.cs` | Implementa IRepositorioUsuarios. Add, ExisteUsuario, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |
| `RepositorioEnMemoria/RepositorioHistorias.cs` | Implementa IRepositorioHistorias. Add, ExisteHistoria, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |
| `RepositorioEnMemoria/RepositorioCategorias.cs` | Implementa IRepositorioCategorias. Add, ExisteCategoria, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |
| `RepositorioEnMemoria/RepositorioOpcion.cs` | Implementa IRepositorioOpcion. Add, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException. Opcion no tiene Id, se indexa por posición. ExisteOpcion está vacía (la misma opción puede repetirse en distintos intermedios, la pertenencia se valida por intermedio) |
| `RepositorioEnMemoria/RepositorioIntermedios.cs` | Implementa IRepositorioIntermedios. Add, ExisteIntermedio, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |
| `RepositorioEnMemoria/RepositorioFinales.cs` | Implementa IRepositorioFinales. Add, ExisteFinal, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |
| `RepositorioEnMemoria/RepositorioLecturas.cs` | Implementa IRepositorioLecturas. Add, ExisteLectura, FindById, FindAll, ObtenerPorUsuario, ObtenerPorHistoria funcionan. Update y Remove lanzan NotImplementedException |

> **Impacto en Add():** los `Add()` de los repositorios de Historia/Categoria/Intermedio/Final/Opcion/Lectura llaman a `Validar()` (ahora implementado), por lo que **todas las altas funcionan de punta a punta**. Solo `RepositorioUsuarios` estaba operativo antes.

## Estado de Implementación

### Completado
- [x] Entidades de dominio (Usuario, Historia, Capitulo, Intermedio, Final, Opcion, Categoria)
- [x] ValueObjects con validación (Email, Password, NombreCompleto, Titulo, Sinopsis, TextoCapitulo, TextoOpcion, Nombre, CantLlegaron)
- [x] Excepciones por entidad
- [x] Interfaces de repositorio (IRepository<T> + IRepositorioUsuarios/Historias/Categorias/Opcion/Intermedios/Finales)
- [x] DTOs para Usuario, Historia, Capitulo, Categoria, Intermedio, Final y Opcion
- [x] Mappers FromDTO para todas las entidades anteriores
- [x] UsuarioMapper.ToDTO (entidad → DTO)
- [x] Casos de uso de Usuarios (AgregarUsuarioCU, EncontrarTodosUsuariosCU, ObtenerUsuarioPorIdCU)
- [x] Repositorios en memoria (parcial: Add, Existe, FindById, FindAll funcionan; Update y Remove lanzan NotImplementedException)
- [x] DTOs sin referencias a entidades de dominio (`HistoriaDTO.Categorias` → `List<CategoriaDTO>`, `HistoriaDTO.Capitulo` → `CapituloDTO`; mappers convierten los anidados)
- [x] Enums de dominio reemplazados por `int` en los DTOs (`HistoriaDTO.Estado`, `UsuarioDTO.Rol`) y casteados a su enum en el Mapper (se elimina la decisión previa de mantener `EstadoHistoria`/`RolesUsuario` en los DTOs)
- [x] `IntermedioDTO`/`FinalDTO` y sus mappers eliminados e integrados en `CapituloDTO` (campo `CantLlegaron`) con `CapituloMapper.FromIntermedioDTO`/`FromFinalDTO`
- [x] `CapituloMapper` copia el `Id` del DTO en `FromIntermedioDTO`/`FromFinalDTO` (resuelve el pendiente "Copiar Id en CapituloMapper/IntermedioMapper/FinalMapper")
- [x] Regresiones del refactor resueltas: `HistoriaMapper` usa `CapituloMapper.FromIntermedioDTO`, `OpcionMapper` expone `FromToIntermedioDTO`/`FromToFinalDTO`, y `CapituloDTO` recupera `Opciones` (compila con 0 errores)
- [x] `IValidable` implementado en los ValueObjects restantes (`NombreCompleto`, `Titulo` de Historia, `Sinopsis`, `Titulo` de Capitulo, `Texto`, `Nombre`, `CantLlegaron`), cada uno lanza la excepción de su contexto (`UsuarioException`, `HistoriaException`, `CapituloException`, `CategoriaException`, `FinalException`)
- [x] Limpieza de `using` redundantes (55 archivos; ImplicitUsings habilitado)
- [x] Quitar referencia `ValueObjects\Intermedio` inexistente en `ProyectoObligatorio/Dominio.csproj`
- [x] `Validar()` implementado en todas las entidades: `Capitulo` `virtual` (Titulo/Texto), `Intermedio`/`Final` `override` (resuelve el CS0108), `Opcion` (Texto, sin CapDestino para evitar ciclos), `Categoria` (Nombre), `Historia` (Titulo/Sinopsis/Categorias/Capitulo). Desbloquea los `Add()` de Historia/Categoria/Intermedio/Final/Opcion
- [x] ToDTO para `HistoriaMapper`, `CategoriaMapper`, `CapituloMapper` (ToIntermedioDTO/ToFinalDTO) y `OpcionMapper` (entidad → DTO)
- [x] `OpcionMapper` unificado en un único `FromDTO` (ternario: `Opciones.Count is 0` → Final, si no Intermedio, casteando a `Capitulo`). Se eliminaron `FromToIntermedioDTO`/`FromToFinalDTO`
- [x] Modelo de `Historia` con `CapituloInicial` + `List<Capitulo> Capitulos` (opción B). Se eliminó `Pertenece` de `Capitulo`/`CapituloDTO`. Pendiente: ligar destinos por referencia (fase 2 del mapper) y regla "no compartir" en repositorio
- [x] `CapituloMapper` unificado en un único `FromDTO`/`ToDTO` (el tipo se infiere: `Opciones.Count == 0` → Final, si no Intermedio; `ToDTO` distingue con `is Final`). `OpcionMapper` y `HistoriaMapper` delegan en él; `LecturaMapper` castea a `Final` (su `FinalAlcanzado` es Final)
- [x] Flujo de `Lectura` completo (salvo presentación): `FechaInicio`/`FechaFin?` como `DateTime`, `IValidable` en la entidad, `LecturaException`, `LecturaDTO`/`LecturaMapper` con DTOs anidados, `IRepositorioLecturas`/`RepositorioLecturas` (con ObtenerPorUsuario/ObtenerPorHistoria) y casos de uso (Agregar, EncontrarTodas, ObtenerPorId)

### Pendiente
- [ ] Completar repositorios en memoria: `Update` y `Remove` de los 6 repositorios
- [ ] Implementar `ExisteOpcion` en `RepositorioOpcion` (método vacío)
- [ ] Agregar más casos de uso (ModificarUsuario, EliminarUsuario, etc.)
- [ ] Implementar casos de uso para Historia/Capitulo/Opción/Categoría
- [ ] Flujo de `Lectura`: falta solo la capa de presentación (Controller + vistas + DI en `Program.cs`)
- [ ] Corregir CS8618 (34 warnings): inicializar propiedades no-nullable con `required`/constructores o hacerlas nullable
- [ ] No exponer `Password.Value` en `UsuarioMapper.ToDTO` (fallo de seguridad) — **no prioritario**, se mantiene por decisión del docente
- [ ] Revisar modelo de `Opcion` (sin Id pero `IRepository<Opcion>` exige `FindById(int)`; `RepositorioOpcion.FindById` usa el id como índice de lista). Nota: `OpcionDTO` ya tiene `Id`, pero `OpcionMapper` no lo traslada
- [ ] Revisar `using` en `HistoriaDTO`/`OpcionDTO` (importan `Dominio.Entidades` pero ya no usan enums de dominio)
- [ ] Revisar semántica de métodos `Existe*` (declarados `void`, lanzan excepción si existe; no devuelven `bool`)
- [ ] `HistoriaMapper.FromDTO`: ligar los `CapDestino` por referencia/Id a las instancias de `Capitulos` (hoy los mapea por copia) y validar "no compartir capítulos entre historias" en `RepositorioHistorias` con `Capitulos.Intersect(...)`
- [ ] Registrar DI de casos de uso en `Presentacion/Program.cs` (`IAgregarUsuario`, `IEncontrarTodosUsuarios`, `IObtenerUsuarioPorId`)
- [ ] Crear `HomeController` (existe `Views/Home/Index.cshtml` sin controlador)
- [ ] Crear vistas y acciones para Usuario/Historia/Categoria/Capitulo (30+ acciones stub con `return View()` sin vistas)
- [ ] Agregar los `IValidable` al UML

## Notas Importantes

### Patrón de DTOs y Mappers
- Los DTOs se crean por entidad (no por ValueObject individual), siguiendo el ejemplo de `HistoriaDTO`/`HistoriaMapper`.
- Cada DTO expone las propiedades como tipos simples (string/int) y el Mapper (`FromDTO`) las convierte en ValueObjects al construir la entidad.
- Los ValueObjects de `Capitulo` (`Titulo`, `Texto`) y `Categoria` (`Nombre`) se representan dentro de sus DTOs de entidad correspondientes.

### Decisiones de diseño
- `Capitulo` es abstracto (no se puede instanciar) y no existe `CapituloDTO` polimórfico: por eso `CapituloMapper` expone un único `FromDTO` (→ `Capitulo`) que **infiere** el tipo concreto por `Opciones.Count == 0` (Final) o `> 0` (Intermedio), y un único `ToDTO` que distingue con `is Final`. Trade-off: el tipo ya no lo decide el invocador sino los datos del DTO.
- No existe un `CapituloDTO` polimórfico: `IntermedioDTO` y `FinalDTO` se eliminaron y se integraron en un único `CapituloDTO` (el campo `CantLlegaron` pertenecía al viejo `FinalDTO`). Intermedio y Final comparten el mismo DTO.
- Los **enums de dominio se representan como `int`** en los DTOs (`HistoriaDTO.Estado`, `UsuarioDTO.Rol`) y se castean a su enum en el Mapper (`(EstadoHistoria) dto.Estado`, `(RolesUsuario) dto.Rol`). Así los DTOs no dependen de los enums de dominio.
- `OpcionMapper` reutiliza `CapituloMapper` (`FromDTO`/`ToDTO`) para el `CapDestino`: la inferencia del tipo (Intermedio/Final) ocurre en `CapituloMapper`.
- `OpcionDTO.CapDestino` ya es de tipo `CapituloDTO` (correcto; antes referenciaba la entidad `Capitulo` y rompía `OpcionMapper`).
- `UsuarioMapper.ToDTO` expone `Password.Value` en texto plano: **fallo de seguridad pendiente** (no exponer el password en el DTO de salida).

### Configuración
- Target Framework: `net10.0`
- Nullable: enabled (34 warnings CS8618 por propiedades sin inicializar)
- ImplicitUsings: enabled

### Referencias entre proyectos
```
Dominio (base)
  ↑
  ├── DTOs (referencia Dominio)
  │     ↑
  │     └── LogicaAplicacion (referencia DTOs + Dominio)
  │
  └── LogicaAccesoDatos (referencia Dominio)
          ↑
          └── Presentacion (referencia Dominio + DTOs + LogicaAplicacion + LogicaAccesoDatos)
```

---
*Última actualización: 07/09/2026*
*Este archivo se mantiene actualizado a medida que se implementan nuevas funcionalidades.*
