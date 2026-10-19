using Dominio.Entidades;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

namespace LogicaAplicacion.CasosDeUso.Usuarios
{
    public class AgregarUsuarioCU : IAgregarUsuario
    {
        private IRepositorioUsuarios repositorio;
        public AgregarUsuarioCU(IRepositorioUsuarios repo)
        {
            repositorio = repo;
        }
        public void Ejecutar(UsuarioDTO dto)
        {
            repositorio.Add(UsuarioMapper.FromDTO(dto));
        }
    }
}