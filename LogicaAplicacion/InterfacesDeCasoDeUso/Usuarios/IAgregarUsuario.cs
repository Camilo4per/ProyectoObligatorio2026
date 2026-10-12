using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios
{
    public interface IRegistrarUsuario
    {
        public void Ejecutar(UsuarioDTO dto);
    }
}