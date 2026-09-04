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
   - DTOs (7): `UsuarioDTO`, `HistoriaDTO`, `CapituloDTO`, `CategoriaDTO`, `IntermedioDTO`, `FinalDTO`, `OpcionDTO`
   - Mappers (7): `UsuarioMapper`, `HistoriaMapper`, `CapituloMapper`, `CategoriaMapper`, `IntermedioMapper`, `FinalMapper`, `OpcionMapper`

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
| `Entidades/Historia.cs` | Entidad con ValueObjects (Titulo, Sinopsis), listado de Categorias y Capitulo |
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
| `ValueObjects/Capitulo/Texto.cs` | Record con validación de longitud (3-2000 chars). Usado por Capitulo y Opcion |
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
| `Capitulo` | Lanza `NotImplementedException` (abstract, base de Intermedio/Final) |
| `Intermedio` | Lanza `NotImplementedException`. Oculta el miembro heredado (warning CS0108) |
| `Final` | Lanza `NotImplementedException`. Oculta el miembro heredado (warning CS0108) |
| `Opcion` | Lanza `NotImplementedException` |
| `Categoria` | Lanza `NotImplementedException` |
| `Historia` | Lanza `NotImplementedException` |

> **Impacto crítico:** solo `Usuario.Validar()` está implementada. Los `Add()` de los repositorios de Historia, Categoria, Intermedio, Final y Opcion llaman a `Validar()`, por lo que **cualquier alta de esas entidades lanza `NotImplementedException` y falla en runtime**.

### DTOs
| Archivo | Descripción |
|---------|-------------|
| `DTOs/DTOs/UsuarioDTO.cs` | Transferencia de datos con Nombre y Apellido separados |
| `DTOs/DTOs/HistoriaDTO.cs` | Transferencia de datos con Titulo y Sinopsis como strings |
| `DTOs/DTOs/CapituloDTO.cs` | Transferencia de datos base del capítulo (Titulo, Texto) |
| `DTOs/DTOs/CategoriaDTO.cs` | Transferencia de datos de categoría (Nombre) |
| `DTOs/DTOs/IntermedioDTO.cs` | Transferencia de datos de capítulo intermedio (Titulo, Texto, Opciones) |
| `DTOs/DTOs/FinalDTO.cs` | Transferencia de datos de capítulo final (Titulo, Texto, CantLlegaron) |
| `DTOs/DTOs/OpcionDTO.cs` | Transferencia de datos de opción (Texto, CapDestino) |
| `DTOs/Mappers/UsuarioMapper.cs` | Conversión DTO → Entidad (FromDTO) |
| `DTOs/Mappers/HistoriaMapper.cs` | Conversión DTO → Entidad (FromDTO) |
| `DTOs/Mappers/CapituloMapper.cs` | Conversión CapituloDTO → Capitulo (FromDTO). Como Capitulo es abstracto, materializa `new Intermedio` |
| `DTOs/Mappers/CategoriaMapper.cs` | Conversión DTO → Entidad (FromDTO) |
| `DTOs/Mappers/IntermedioMapper.cs` | Conversión DTO → Entidad (FromDTO) |
| `DTOs/Mappers/FinalMapper.cs` | Conversión DTO → Entidad (FromDTO) |
| `DTOs/Mappers/OpcionMapper.cs` | Conversión DTO → Entidad (FromDTO). Reutiliza CapituloMapper.FromDTO para CapDestino |

### LogicaAplicacion
| Archivo | Descripción |
|---------|-------------|
| `CasosDeUso/Usuarios/AgregarUsuarioCU.cs` | Implementado (Add + UsuarioMapper.FromDTO) |
| `CasosDeUso/Usuarios/EncontrarTodosLosUsuariosCU.cs` | Implementado (FindAll + ToDTO). Clase real: `EncontrarTodosUsuariosCU` |
| `CasosDeUso/Usuarios/ObtenerUsuarioPorIdCU.cs` | Implementado (FindById + ToDTO) |
| `InterfacesDeCasoDeUso/Usuarios/IAgregarUsuario.cs` | Interfaz: void Ejecutar(UsuarioDTO) |
| `InterfacesDeCasoDeUso/Usuarios/IEncontrarTodosLosUsuarios.cs` | Interfaz: List<UsuarioDTO> Ejecutar(). Interfaz real: `IEncontrarTodosUsuarios` |
| `InterfacesDeCasoDeUso/Usuarios/IObtenerUsuarioPorId.cs` | Interfaz: UsuarioDTO Ejecutar(int id) |

### LogicaAccesoDatos
| Archivo | Descripción |
|---------|-------------|
| `RepositorioEnMemoria/RepositorioUsuarios.cs` | Implementa IRepositorioUsuarios. Add, ExisteUsuario, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |
| `RepositorioEnMemoria/RepositorioHistorias.cs` | Implementa IRepositorioHistorias. Add, ExisteHistoria, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |
| `RepositorioEnMemoria/RepositorioCategorias.cs` | Implementa IRepositorioCategorias. Add, ExisteCategoria, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |
| `RepositorioEnMemoria/RepositorioOpcion.cs` | Implementa IRepositorioOpcion. Add, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException. Opcion no tiene Id, se indexa por posición. ExisteOpcion está vacía (la misma opción puede repetirse en distintos intermedios, la pertenencia se valida por intermedio) |
| `RepositorioEnMemoria/RepositorioIntermedios.cs` | Implementa IRepositorioIntermedios. Add, ExisteIntermedio, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |
| `RepositorioEnMemoria/RepositorioFinales.cs` | Implementa IRepositorioFinales. Add, ExisteFinal, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |

> **Impacto en Add():** aunque los métodos `Add` existen codificados, los de Historia/Categoria/Intermedio/Final/Opcion llaman a `Validar()` (que lanza `NotImplementedException`), por lo que **en runtime fallan al añadir**. Solo `RepositorioUsuarios.Add()` funciona de punta a punta.

## Estado de Implementación

### Completado
- [x] Entidades de dominio (Usuario, Historia, Capitulo, Intermedio, Final, Opcion, Categoria)
- [x] ValueObjects con validación (Email, Password, NombreCompleto, Titulo, Sinopsis, Texto, Nombre, CantLlegaron)
- [x] Excepciones por entidad
- [x] Interfaces de repositorio (IRepository<T> + IRepositorioUsuarios/Historias/Categorias/Opcion/Intermedios/Finales)
- [x] DTOs para Usuario, Historia, Capitulo, Categoria, Intermedio, Final y Opcion
- [x] Mappers FromDTO para todas las entidades anteriores
- [x] UsuarioMapper.ToDTO (entidad → DTO)
- [x] Casos de uso de Usuarios (AgregarUsuarioCU, EncontrarTodosUsuariosCU, ObtenerUsuarioPorIdCU)
- [x] Repositorios en memoria (parcial: Add, Existe, FindById, FindAll funcionan; Update y Remove lanzan NotImplementedException)
- [x] DTOs sin referencias a entidades de dominio (`HistoriaDTO.Categorias` → `List<CategoriaDTO>`, `HistoriaDTO.Capitulo` → `CapituloDTO`, `IntermedioDTO.Opciones` → `List<OpcionDTO>`; mappers convierten los anidados)

### Pendiente
- [ ] Implementar lógica en `IValidable.Validar()` por entidad (solo Usuario tiene implementación; el resto lanza `NotImplementedException`). **Desbloquea el `Add()` de Historia/Categoria/Intermedio/Final/Opcion**
- [ ] Completar repositorios en memoria: `Update` y `Remove` de los 6 repositorios
- [ ] Implementar `ExisteOpcion` en `RepositorioOpcion` (método vacío)
- [ ] Agregar más casos de uso (ModificarUsuario, EliminarUsuario, etc.)
- [ ] Implementar casos de uso para Historia/Capitulo/Opción/Categoría
- [ ] Corregir `Validar()` de `Intermedio`/`Final` (ocultan miembro heredado → CS0108). Hacer `virtual` en `Capitulo` y `override` en hijos
- [ ] Corregir CS8618 (34 warnings): inicializar propiedades no-nullable con `required`/constructores o hacerlas nullable
- [ ] Eliminar referencias a entidades de dominio en DTOs (PENDIENTE PARCIAL: ya se cambiaron `Categorias`/`Capitulo`/`Opciones` a DTOs y los mappers convierten; por decisión se MANTIENEN los enums de dominio `EstadoHistoria` en `HistoriaDTO` y `RolesUsuario` en `UsuarioDTO`)
- [ ] Copiar `Id` en `CapituloMapper`/`IntermedioMapper`/`FinalMapper` (los mappers no trasladan el Id del DTO)
- [ ] No exponer `Password.Value` en `UsuarioMapper.ToDTO` (fallo de seguridad)
- [ ] Revisar modelo de `Opcion` (sin Id pero `IRepository<Opcion>` exige `FindById(int)`; `RepositorioOpcion.FindById` usa el id como índice de lista)
- [ ] Revisar semántica de métodos `Existe*` (declarados `void`, lanzan excepción si existe; no devuelven `bool`)
- [ ] Registrar DI de casos de uso en `Presentacion/Program.cs` (`IAgregarUsuario`, `IEncontrarTodosUsuarios`, `IObtenerUsuarioPorId`)
- [ ] Crear `HomeController` (existe `Views/Home/Index.cshtml` sin controlador)
- [ ] Crear vistas y acciones para Usuario/Historia/Categoria/Capitulo (30+ acciones stub con `return View()` sin vistas)
- [ ] Quitar referencia `ValueObjects\Intermedio` inexistente en `ProyectoObligatorio/ProyectoObligatorio.csproj`
- [ ] Limpiar `using` redundantes (ImplicitUsings habilitado)
- [ ] Agregar los `IValidable` al UML

## Notas Importantes

### Patrón de DTOs y Mappers
- Los DTOs se crean por entidad (no por ValueObject individual), siguiendo el ejemplo de `HistoriaDTO`/`HistoriaMapper`.
- Cada DTO expone las propiedades como tipos simples (string/int) y el Mapper (`FromDTO`) las convierte en ValueObjects al construir la entidad.
- Los ValueObjects de `Capitulo` (`Titulo`, `Texto`) y `Categoria` (`Nombre`) se representan dentro de sus DTOs de entidad correspondientes.

### Decisiones de diseño
- `CapituloMapper.FromDTO` materializa `new Intermedio` porque `Capitulo` es abstracto (no se puede instanciar). Esto implica que un `CapituloDTO` genérico siempre se convierte en `Intermedio`; para crear un `Final` debe usarse `FinalMapper`.
- `OpcionMapper` reutiliza `CapituloMapper.FromDTO` para el `CapDestino`.
- `OpcionDTO.CapDestino` ya es de tipo `CapituloDTO` (correcto; antes referenciaba la entidad `Capitulo` y rompía `OpcionMapper`).
- `UsuarioMapper.ToDTO` expone `Password.Value` en texto plano: **fallo de seguridad pendiente** (no exponer el password en el DTO de salida).
- Los DTOs usan tipos DTO para los anidados de entidades (`Categorias`, `Capitulo`, `Opciones`), pero **mantienen los enums de dominio** (`EstadoHistoria`, `RolesUsuario`) por decisión, ya que el proyecto `DTOs` referencia `Dominio`.

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
*Última actualización: 04/09/2026*
*Este archivo se mantiene actualizado a medida que se implementan nuevas funcionalidades.*
