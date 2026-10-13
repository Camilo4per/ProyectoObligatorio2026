using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios
{
    public interface ILoginUsuario
    {
        public UsuarioDTO Ejecutar(string usuario, string password);
    }
}