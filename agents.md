# Documentación del Proyecto - Arquitectura DDD

## Descripción General
Proyecto de facultad utilizando arquitectura DDD (Domain-Driven Design). El sistema gestiona usuarios, historias interactivas y capítulos con opciones de destino.

## Arquitectura Actual
El proyecto sigue una arquitectura en 3 capas con separación por proyectos:

```
Dominio (Entidades + ValueObjects) ←→ DTOs ←→ LogicaAplicacion (Casos de Uso)
                                              ←→ LogicaAccesoDatos (Repositorios)
```

### Capas:

1. **Dominio** (`ProyectoObligatorio/`)
   - Entidades: `Usuario`, `Historia`, `Capitulo` (abstract), `Intermedio`, `Final`, `Opcion`, `Categoria`
   - Enums: `EstadoHistoria` (Borrador, Publicada), `RolesUsuario` (Administrador, Usuario)
   - ValueObjects: `Email`, `Password`, `NombreCompleto`, `Titulo`, `Sinopsis`
   - Excepciones: `UsuarioException`, `CapituloException`, `CategoriaException`, `FinalException`, `HistoriaException`, `IntermedioException`, `OpcionException`
   - Interfaces: `IValidable` (internal)
   - InterfaceDeRepositorio: `IRepository<T>`, `IRepositorioUsuarios`

2. **DTOs** (`DTOs/`)
   - DTOs: `UsuarioDTO`, `HistoriaDTO`
   - Mappers: `UsuarioMapper`, `HistoriaMapper`

3. **LogicaAplicacion** (`LogicaAplicacion/`)
   - Casos de uso (stubs vacíos): `AgregarUsuarioCU`, `EncontrarTodosLosUsuariosCU`, `ObtenerUsuarioPorIdCU`
   - Interfaces de casos de uso (stubs vacíos): `IAgregarUsuario`, `IEncontrarTodosLosUsuarios`, `IObtenerUsuarioPorId`

4. **LogicaAccesoDatos** (`LogicaAccesoDatos/`)
   - RepositorioEnMemoria: `RepositorioUsuarios` (parcialmente implementado)

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
| `InterfaceDeRepositorio/IRepository.cs` | Interfaz genérica: Add, Update, Remove, FindById |
| `InterfaceDeRepositorio/IRepositorioUsuarios.cs` | Extiende IRepository<Usuario>, agrega ExisteUsuario |

### DTOs
| Archivo | Descripción |
|---------|-------------|
| `DTOs/DTOs/UsuarioDTO.cs` | Transferencia de datos con Nombre y Apellido separados |
| `DTOs/DTOs/HistoriaDTO.cs` | Transferencia de datos con Titulo y Sinopsis como strings |
| `DTOs/Mappers/UsuarioMapper.cs` | Conversión DTO → Entidad (FromDTO) |
| `DTOs/Mappers/HistoriaMapper.cs` | Conversión DTO → Entidad (FromDTO) |

### LogicaAplicacion
| Archivo | Descripción |
|---------|-------------|
| `CasosDeUso/Usuario/AgregarUsuarioCU.cs` | Stub vacío |
| `CasosDeUso/Usuario/EncontrarTodosLosUsuariosCU.cs` | Stub vacío |
| `CasosDeUso/Usuario/ObtenerUsuarioPorIdCU.cs` | Stub vacío |
| `InterfacesDeCasoDeUso/Usuario/IAgregarUsuario.cs` | Stub vacío |
| `InterfacesDeCasoDeUso/Usuario/IEncontrarTodosLosUsuarios.cs` | Stub vacío |
| `InterfacesDeCasoDeUso/Usuario/IObtenerUsuarioPorId.cs` | Stub vacío |

### LogicaAccesoDatos
| Archivo | Descripción |
|---------|-------------|
| `RepositorioEnMemoria/RepositorioUsuarios.cs` | Implementación parcial: Add, Existe, Get, GetAll funcionan. Update y Delete lanzan NotImplementedException. NO implementa IRepositorioUsuarios |

## Estado de Implementación

### Completado
- [x] Entidades de dominio (Usuario, Historia, Capitulo, Intermedio, Final, Opcion, Categoria)
- [x] ValueObjects con validación (Email, Password, NombreCompleto, Titulo, Sinopsis)
- [x] Excepciones por entidad
- [x] Interfaces de repositorio (IRepository<T>, IRepositorioUsuarios)
- [x] DTOs para Usuario e Historia
- [x] Mappers FromDTO para Usuario e Historia
- [x] Repositorio en memoria (parcial: Add, Existe, Get, GetAll)

### Pendiente
- [ ] Implementar casos de uso (stubs vacíos)
- [ ] Completar RepositorioUsuarios (Update, Delete)
- [ ] Hacer que RepositorioUsuarios implemente IRepositorioUsuarios
- [ ] Agregar más casos de uso (ModificarUsuario, EliminarUsuario, etc.)
- [ ] Agregar más repositorios (Historia, Capitulo, etc.)
- [ ] Implementar lógica en `IValidable.Validar()` por entidad (solo Usuario tiene implementación parcial)
- [ ] Agregar persistencia (base de datos)
- [ ] Implementar casos de uso para Historia/Capitulo/Opción
- [ ] Agregar ToDTO en los Mappers (actualmente solo tienen FromDTO)

## Notas Importantes

### Bug conocido
- `Intermedio.cs:11` - El getter de `Opciones` llama a `new List<Opcion>(Opciones)` creando recursión infinita (StackOverflowException). Debería ser un campo privado backing.

### Configuración
- Target Framework: `net10.0`
- Nullable: enabled
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
```

---
*Última actualización: 03/09/2026*
*Este archivo se mantiene actualizado a medida que se implementan nuevas funcionalidades.*
