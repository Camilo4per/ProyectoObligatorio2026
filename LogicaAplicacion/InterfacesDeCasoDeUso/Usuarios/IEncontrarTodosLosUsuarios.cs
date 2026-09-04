using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios
{
    public interface IEncontrarTodosUsuarios
    {
        public List<UsuarioDTO> Ejecutar();
    }
}
