using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

namespace LogicaAplicacion.CasosDeUso.Usuarios
{
    public class LoginUsuarioCU : ILoginUsuario
    {
        private IRepositorioUsuarios repositorio;

        public LoginUsuarioCU(IRepositorioUsuarios repo)
        {
            repositorio = repo;
        }

        public UsuarioDTO Ejecutar(string usuario, string password)
        {
            List<Usuario> todos = repositorio.FindAll();

            foreach (Usuario u in todos)
            {
                if (u.NombreUsuario == usuario && u.Password.Verify(password))
                {
                    return UsuarioMapper.ToDTO(u);
                }
            }

            throw new UsuarioException("Usuario o contraseña incorrectos");
        }
    }
}