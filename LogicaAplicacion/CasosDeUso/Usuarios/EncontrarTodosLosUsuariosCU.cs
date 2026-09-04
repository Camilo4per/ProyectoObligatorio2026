using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Dominio.Entidades;

namespace LogicaAplicacion.CasosDeUso.Usuarios
{
    public class EncontrarTodosUsuariosCU : IEncontrarTodosUsuarios
    {
        private IRepositorioUsuarios repositorio;
        public EncontrarTodosUsuariosCU(IRepositorioUsuarios repo)
        {
            repositorio = repo;
        }

        public List<UsuarioDTO> Ejecutar()
        {
            List<Usuario> usuarios = repositorio.FindAll();
            List<UsuarioDTO> aRetornar = new List<UsuarioDTO>();
            foreach (Usuario usuario in usuarios)
            {
                aRetornar.Add(UsuarioMapper.ToDTO(usuario));
            }
            return aRetornar;
        }
    }
}
