using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios
{
    public interface IAgregarUsuario
    {
        public void Ejecutar(UsuarioDTO dto);
    }
}
