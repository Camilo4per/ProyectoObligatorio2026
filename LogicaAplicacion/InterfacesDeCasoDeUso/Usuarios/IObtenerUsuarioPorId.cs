using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios
{
    public interface IObtenerUsuarioPorId
    {
        public UsuarioDTO Ejecutar(int id);
    }
}
