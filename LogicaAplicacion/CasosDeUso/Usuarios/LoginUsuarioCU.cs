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

        public UsuarioDTO Ejecutar(string email, string password)
        {
            List<Usuario> todos = repositorio.FindAll();
            Usuario usuario = todos.FirstOrDefault(u => u.Email.Value == email)
                ?? throw new UsuarioException("Email o contraseña incorrectos");

            if (!usuario.Password.Verify(password))
                throw new UsuarioException("Email o contraseña incorrectos");

            return UsuarioMapper.ToDTO(usuario);
        }
    }
}