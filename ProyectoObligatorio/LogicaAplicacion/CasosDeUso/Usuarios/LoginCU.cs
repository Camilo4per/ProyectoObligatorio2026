using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

namespace LogicaAplicacion.CasosDeUso.Usuarios
{
    public class LoginCU : ILoginCU
    {
        private IRepositorioUsuarios repositorio;

        public LoginCU(IRepositorioUsuarios repo)
        {
            repositorio = repo;
        }

        public UsuarioDTO Ejecutar(string usuario, string password)
        {
            Usuario? u = repositorio.FindByNombreUsuario(usuario);

            if (u != null && u.Password.Verify(password))
            {
                return UsuarioMapper.ToDTO(u);
            }

            throw new UsuarioException("Usuario o contraseña incorrectos");
        }
    }
}
