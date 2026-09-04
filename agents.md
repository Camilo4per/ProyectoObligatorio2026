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
   - ValueObjects: `Email`, `Password`, `NombreCompleto`, `Titulo`, `Sinopsis`, `Texto`, `Nombre`, `CantLlegaron`
   - Excepciones: `UsuarioException`, `CapituloException`, `CategoriaException`, `FinalException`, `HistoriaException`, `IntermedioException`, `OpcionException`
   - Interfaces: `IValidable` (internal)
   - InterfaceDeRepositorio: `IRepository<T>`, `IRepositorioUsuarios`
   - Varias entidades implementan `IValidable` (Intermedio, Historia, etc.), aunque sus `Validar()` aún lanzan `NotImplementedException`

2. **DTOs** (`DTOs/`)
   - DTOs: `UsuarioDTO`, `HistoriaDTO`
   - Mappers: `UsuarioMapper`, `HistoriaMapper`

3. **LogicaAplicacion** (`LogicaAplicacion/`)
   - Casos de uso (stubs vacíos): `AgregarUsuarioCU`, `EncontrarTodosLosUsuariosCU`, `ObtenerUsuarioPorIdCU`
   - Interfaces de casos de uso (stubs vacíos): `IAgregarUsuario`, `IEncontrarTodosLosUsuarios`, `IObtenerUsuarioPorId`

4. **LogicaAccesoDatos** (`LogicaAccesoDatos/`)
   - RepositorioEnMemoria: `RepositorioUsuarios`, `RepositorioHistorias`, `RepositorioCategorias`, `RepositorioOpciones`, `RepositorioIntermedios`, `RepositorioFinales` (parcialmente implementados)

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
| `InterfaceDeRepositorio/IRepositorioOpciones.cs` | Extiende IRepository<Opcion>, agrega ExisteOpcion |
| `InterfaceDeRepositorio/IRepositorioIntermedios.cs` | Extiende IRepository<Intermedio>, agrega ExisteIntermedio |
| `InterfaceDeRepositorio/IRepositorioFinales.cs` | Extiende IRepository<Final>, agrega ExisteFinal |

### Dominio - Implementación IValidable
| Entidad | Estado de Validar() |
|---------|--------------------|
| `Usuario` | Implementada (valida NombreUsuario) |
| `Intermedio` | Lanza `NotImplementedException` |
| `Historia` | Llama ToString() de ValueObjects (validación indirecta) |

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
| `CasosDeUso/Usuarios/AgregarUsuarioCU.cs` | Stub vacío |
| `CasosDeUso/Usuarios/EncontrarTodosLosUsuariosCU.cs` | Stub vacío |
| `CasosDeUso/Usuarios/ObtenerUsuarioPorIdCU.cs` | Stub vacío |
| `InterfacesDeCasoDeUso/Usuarios/IAgregarUsuario.cs` | Stub vacío |
| `InterfacesDeCasoDeUso/Usuarios/IEncontrarTodosLosUsuarios.cs` | Stub vacío |
| `InterfacesDeCasoDeUso/Usuarios/IObtenerUsuarioPorId.cs` | Stub vacío |

### LogicaAccesoDatos
| Archivo | Descripción |
|---------|-------------|
| `RepositorioEnMemoria/RepositorioUsuarios.cs` | Implementa IRepositorioUsuarios. Add, ExisteUsuario, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |
| `RepositorioEnMemoria/RepositorioHistorias.cs` | Implementa IRepositorioHistorias. Add, ExisteHistoria, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |
| `RepositorioEnMemoria/RepositorioCategorias.cs` | Implementa IRepositorioCategorias. Add, ExisteCategoria, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |
| `RepositorioEnMemoria/RepositorioOpciones.cs` | Implementa IRepositorioOpciones. Add, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException. Opcion no tiene Id, se indexa por posición. ExisteOpcion está vacía (la misma opción puede repetirse en distintos intermedios, la pertenencia se valida por intermedio) |
| `RepositorioEnMemoria/RepositorioIntermedios.cs` | Implementa IRepositorioIntermedios. Add, ExisteIntermedio, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |
| `RepositorioEnMemoria/RepositorioFinales.cs` | Implementa IRepositorioFinales. Add, ExisteFinal, FindById, FindAll funcionan. Update y Remove lanzan NotImplementedException |

## Estado de Implementación

### Completado
- [x] Entidades de dominio (Usuario, Historia, Capitulo, Intermedio, Final, Opcion, Categoria)
- [x] ValueObjects con validación (Email, Password, NombreCompleto, Titulo, Sinopsis, Texto, Nombre, CantLlegaron)
- [x] Excepciones por entidad
- [x] Interfaces de repositorio (IRepository<T> + IRepositorioUsuarios/Historias/Categorias/Opciones/Intermedios/Finales)
- [x] DTOs para Usuario, Historia, Capitulo, Categoria, Intermedio, Final y Opcion
- [x] Mappers FromDTO para todas las entidades anteriores
- [x] Repositorios en memoria (parcial: Add, Existe, FindById, FindAll funcionan; Update y Remove lanzan NotImplementedException)

### Pendiente
- [ ] Implementar casos de uso (stubs vacíos)
- [ ] Completar repositorios en memoria (Update y Remove lanzan NotImplementedException)
- [ ] Agregar más casos de uso (ModificarUsuario, EliminarUsuario, etc.)
- [ ] Implementar lógica en `IValidable.Validar()` por entidad (solo Usuario tiene implementación parcial)
- [ ] Implementar casos de uso para Historia/Capitulo/Opción
- [ ] Agregar los `IValidable` al UML

## Notas Importantes

### Patrón de DTOs y Mappers
- Los DTOs se crean por entidad (no por ValueObject individual), siguiendo el ejemplo de `HistoriaDTO`/`HistoriaMapper`.
- Cada DTO expone las propiedades como tipos simples (string/int) y el Mapper (`FromDTO`) las convierte en ValueObjects al construir la entidad.
- Los ValueObjects de `Capitulo` (`Titulo`, `Texto`) y `Categoria` (`Nombre`) se representan dentro de sus DTOs de entidad correspondientes.

### Decisiones de diseño
- `CapituloMapper.FromDTO` materializa `new Intermedio` porque `Capitulo` es abstracto (no se puede instanciar). Esto implica que un `CapituloDTO` genérico siempre se convierte en `Intermedio`; para crear un `Final` debe usarse `FinalMapper`.
- `OpcionMapper` reutiliza `CapituloMapper.FromDTO` para el `CapDestino`.

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
