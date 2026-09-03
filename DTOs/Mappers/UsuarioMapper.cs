using Dominio.Entidades;
using Dominio.ValueObjects.Usuario;
using DTOs.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

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
                Rol = dto.Rol,
                Email = new Email(dto.Email),
                Password = new Password(dto.Password)
            };
        }
    }
}
