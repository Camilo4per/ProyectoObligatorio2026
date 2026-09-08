using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Dominio.InterfaceDeRepositorio;

namespace LogicaAplicacion.CasosDeUso.Usuarios
{
    public class ObtenerUsuarioPorIdCU : IObtenerUsuarioPorId
    {
        private IRepositorioUsuarios repositorio;
        public ObtenerUsuarioPorIdCU(IRepositorioUsuarios repo)
        {
            repositorio = repo;
        }
        public UsuarioDTO Ejecutar(int id)
        {
            Dominio.Entidades.Usuario usuario = repositorio.FindById(id);
            return UsuarioMapper.ToDTO(usuario);
        }
    }
}
