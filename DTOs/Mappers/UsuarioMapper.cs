using Dominio.Entidades;
using Dominio.ValueObjects.Usuario;
using DTOs.DTOs;

namespace DTOs.Mappers
{
public class UsuarioMapper
    {
        public static Usuario FromDTO(UsuarioDTO dto)
        {
            return new Usuario
            {
                Id = dto.Id,
                NombreCompleto = new NombreCompleto(dto.Nombre, dto.Apellido),
                NombreUsuario = dto.NombreUsuario,
                Rol = (RolesUsuario) dto.Rol,
                Email = new Email(dto.Email),
                Password = new Password(dto.Password)
            };
        }

        public static UsuarioDTO ToDTO(Usuario usuario)
        {
            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.NombreCompleto.Nombre,
                Apellido = usuario.NombreCompleto.Apellido,
                NombreUsuario = usuario.NombreUsuario,
                Rol = (int) usuario.Rol,
                Email = usuario.Email.Value,
                Password = usuario.Password.Value
            };
        }
    }
}
