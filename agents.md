# Documentación del Proyecto - Arquitectura DDD

## Descripción General
Proyecto de facultad utilizando arquitectura DDD (Domain-Driven Design). El sistema gestiona usuarios, historias interactivas y capítulos con opciones de destino.

## Arquitectura Actual
El proyecto sigue una arquitectura en 3 capas clásicas DDD:

```
Dominio (Entidades + ValueObjects) ←→ DTOs ←→ LogicaAplicacion (Casos de Uso)
```

### Capas:

1. **Dominio** (`ProyectoObligatorio/Dominio`)
   - Entidades: `Usuario`, `Historia`, `Capitulo`, `Opcion`, `Intermedio`, `Final`
   - ValueObjects: `Email`, `Password`, `NombreCompleto`
   - Excepciones: `UsuarioException`
   - Interfaces: `IValidable`

2. **DTOs** (`DTOs`)
   - Objetos de transferencia de datos
   - Mappers para conversión entre DTO y Entidad

3. **LogicaAplicacion** (`LogicaAplicacion`)
   - Casos de uso (`CasosDeUso`)
   - Interfaces de casos de uso (`InterfacesDeCasoDeUso`)
   - Acceso al sistema mediante Singleton (`Sistema`)

## Estructura de Archivos Importantes

### Dominio
- `Entidades/Usuario.cs` - Entidad principal con ValueObjects integrados
- `ValueObjects/Usuario/Email.cs` - Validación de formato email
- `ValueObjects/Usuario/Password.cs` - Validación de fortaleza y enmascaramiento
- `ValueObjects/Usuario/NombreCompleto.cs` - Nombre y apellido con ToString combinado
- `InterfacesDelDominio/IValidable.cs` - Interfaz de validación
- `Excepciones/UsuarioException.cs` - Excepciones específicas

### DTOs
- `DTOs/DTOs/UsuarioDTO.cs` - Transferencia de datos (ahora con Nombre y Apellido separados)
- `DTOs/Mappers/UsuarioMapper.cs` - Conversión DTO ↔ Entidad

### LogicaAplicacion
- `CasosDeUso/Usuario/AgregarUsuarioCU.cs` - Caso de uso agregar usuario
- `InterfacesDeCasoDeUso/Usuario/IAgregarUsuario.cs` - Interfaz del caso de uso
- `Sistema.cs` - Singleton que mantiene el estado en memoria

## Patrones Implementados

### Value Objects
- **Encapsulan validación y formato**
- Ejemplo: `Password` valida longitud y complejidad; `Email` valida formato; `NombreCompleto` combina nombre y apellido

### Mapper
- **Convierte DTOs a Entidades y viceversa**
- Centraliza la lógica de transformación entre capas

### Singleton (Sistema)
- **Mantiene estado global en memoria**
- Acceso mediante `Sistema.Instancia`

## Próximos Pasos / Pendientes

- [ ] Implementar más casos de uso (ModificarUsuario, EliminarUsuario, etc.)
- [ ] Agregar repositorios formalizados
- [ ] Implementar lógica en `IValidable.Validar()` por entidad
- [ ] Agregar persistencia (base de datos)
- [ ] Implementar casos de uso para Historia/Capitulo/Opción

## Referencias Rápidas

| Archivo | Línea clave | Descripción |
|---------|-------------|-------------|
| `ProyectoObligatorio/Entidades/Usuario.cs` | 13 | `public NombreCompleto NombreCompleto { get; init; }` |
| `DTOs/Mappers/UsuarioMapper.cs` | 13 | `new NombreCompleto(dto.Nombre, dto.Apellido)` |
| `ProyectoObligatorio/ValueObjects/Usuario/NombreCompleto.cs` | 24 | `ToString()` combina nombre y apellido |
| `LogicaAplicacion/CasosDeUso/Usuario/AgregarUsuarioCU.cs` | 17-21 | Uso de `Sistema.Instancia` y `UsuarioMapper.FromDTO` |
| `DTOs/DTOs/UsuarioDTO.cs` | 8-9 | `public string Nombre` y `public string Apellido` separados |

---
*Última actualización: 03/09/2026*
*Este archivo se mantiene actualizado a medida que se implementan nuevas funcionalidades.*