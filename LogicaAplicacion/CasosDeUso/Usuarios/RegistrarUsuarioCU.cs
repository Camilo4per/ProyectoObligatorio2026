using Dominio.Entidades;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

namespace LogicaAplicacion.CasosDeUso.Usuarios
{
    public class RegistrarUsuarioCU : IRegistrarUsuario
    {
        private IRepositorioUsuarios repositorio;
        public RegistrarUsuarioCU(IRepositorioUsuarios repo)
        {
            repositorio = repo;
        }
        public void Ejecutar(UsuarioDTO dto)
        {
            // Sobreescribo rol para que siempre sea usuario y no puedan registrarse como administrador
            dto.Rol = (int) RolesUsuario.Usuario;
            repositorio.Add(UsuarioMapper.FromDTO(dto));
        }
    }
}