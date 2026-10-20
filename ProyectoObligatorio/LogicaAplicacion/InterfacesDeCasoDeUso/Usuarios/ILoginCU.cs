using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios
{
    public interface ILoginCU
    {
        public UsuarioDTO Ejecutar(LoginDTO dto);
    }
}
